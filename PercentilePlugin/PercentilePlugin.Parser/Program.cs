using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;
using NLog;
using NLog.Config;
using NLog.Targets;
using PercentilePlugin.Shared;

namespace PercentilePlugin
{
    internal class Program
    {
        public static Logger Logger;

        private static List<ClassJob> jobs;
        private static List<Instance> instances;
        private static PercentileData percentileData;

        private static readonly string APIKey = "3dd4ec9f05c333b48d73fc7ea577a6c5";

        public static async Task Main(string[] args)
        {
            var config = new LoggingConfiguration();
            var consoleTarget = new ColoredConsoleTarget("target1")
            {
                Layout = @"${date:format=HH\:mm\:ss} ${level} ${message} ${exception}"
            };
            config.AddTarget(consoleTarget);

            var fileTarget = new FileTarget("target2")
            {
                FileName = "${basedir}/file.txt",
                Layout = "${longdate} ${level} ${message}  ${exception}"
            };
            config.AddTarget(fileTarget);
            config.AddRuleForOneLevel(LogLevel.Error, fileTarget); // only errors to file
            config.AddRuleForAllLevels(consoleTarget); // all to console

            LogManager.Configuration = config;

            Logger = LogManager.GetLogger("Main");

            // Backup data before we begin.
            if (File.Exists("parsedata.bin"))
            {
                if (File.Exists("parsedata.bak")) File.Delete("parsedata.bak");
                Logger.Log(LogLevel.Info, "Backing Up Parse Data.");
                File.Copy("parsedata.bin", "parsedata.bak");
            }

            Logger.Log(LogLevel.Info, "Loading Parse Data.");
            percentileData = PercentileData.Load("parsedata.bin");
            Logger.Log(LogLevel.Info, "Parse Data Loaded, Last Update: " + percentileData.LastUpdated);


            Logger.Log(LogLevel.Info, "Obtaining Job/Class Data.");
            await BuildClasses(APIKey);
            Logger.Log(LogLevel.Info, "Job/Class Data Obtained.");
            Logger.Log(LogLevel.Info, "Obtaining Zone/Instance Data.");
            await BuildInstances(APIKey);
            Logger.Log(LogLevel.Info, "Zone/Instance Data Obtained.");
            Logger.Log(LogLevel.Info, "Obtaining latest Parse Data.");
            await BuildPercentiles(APIKey);
            Logger.Log(LogLevel.Info, "Latest Parse Data Obtained.");
            Logger.Log(LogLevel.Info, "Cleaning Up New Data.");

            // Remove Duplicated Entries.
            var distinctDictionary =
                new Dictionary<string, Dictionary<string, List<double>>>(percentileData.Rankings);


            Logger.Log(LogLevel.Info, "Removing Unused Fights.");

            // Remove unused fights
            foreach (var encounter in distinctDictionary.Keys)
            {
                var used = false;
                foreach (var instance in instances)
                    if (instance.Encounters.FirstOrDefault(e => e.Value.Name.ToLower() == encounter.ToLower()).Value !=
                        null)
                    {
                        Logger.Log(LogLevel.Debug, encounter + " is used.");
                        used = true;
                    }

                if (!used) Logger.Log(LogLevel.Warn, "Encounter: " + encounter + " Is no longer needed.");
            }

            Logger.Log(LogLevel.Info, "Unused Fights Removed.");

            percentileData.Rankings = distinctDictionary;

            Logger.Log(LogLevel.Info, "Saving Parse Data.");

            // Save
            var file = new FileStream("parsedata.bin", FileMode.OpenOrCreate);
            using (var writer = new BsonWriter(file))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(writer, percentileData);
            }
            file.Close();
            File.WriteAllText("parsejson.json", JsonConvert.SerializeObject(percentileData, Formatting.Indented));
            Logger.Log(LogLevel.Info, "Parse Data Saved.");
            Console.ReadKey();
        }

        public static double GetRealPercentile(double DPS, double[] sequence)
        {
            if (sequence.Length == 0) return 100;
            Array.Sort(sequence);
            var l = 0;
            var r = sequence.Length - 1;
            var index = sequence.Length / 2;

            while (l <= r)
            {
                index = l + (r - l) / 2;

                if (sequence[index] < DPS)
                {
                    if (l == r)
                    {
                        index = index + 1;
                    }
                    l = index + 1;
                }
                else
                {
                    r = index - 1;
                }
             }
            double percentile_calc = 0.0;
            percentile_calc = 100 * index / sequence.Length;
            if (percentile_calc <= 99.994)
            {
                return Math.Floor(percentile_calc);
            }
            else
            {
                return 100;
            }
        }

        public static async Task<bool> BuildClasses(string apiKey)
        {
            if (jobs != null)
                jobs.Clear(); // Safety net that should never be needed.
            else
                jobs = new List<ClassJob>();

            try
            {
                var request = WebRequest.Create("https://cn.fflogs.com:443/v1/classes?api_key=" + apiKey);
                var response = await request.GetResponseAsync();
                if (response.GetResponseStream() != null)
                    using (var reader =
                        new StreamReader(response.GetResponseStream() ?? throw new InvalidOperationException()))
                    {
                        var json = reader.ReadToEnd();
                        var array = JArray.Parse(json);
                        if (array.HasValues)
                        {
                            var obj = array.First;
                            if (obj.HasValues)
                            {
                                var specs = obj["specs"].ToObject<JArray>();
                                if (specs.HasValues)
                                    for (var i = 0; i < specs.Count; i++)
                                    {
                                        var job = specs[i];
                                        var cjob = new ClassJob();
                                        cjob.Key = job["id"].ToObject<int>();
                                        cjob.Name = Convert.ToString(job["name"]);
                                        cjob.Abbrveiation = ClassJob.NameToAbbr(cjob.Name).ToUpper();
                                        jobs.Add(cjob);
                                        Logger.Log(LogLevel.Info, string.Format("Parsed Job: {0}. key is {1}", cjob.Name, cjob.Key));
                                    }
                            }
                        }
                    }

                return await Task.FromResult(true);
            }
            catch (InvalidOperationException invalidOperation)
            {
                Logger.Log(LogLevel.Fatal, "Invalid Operation Occurred, Response Stream for Jobs is null.");
                Logger.Log(LogLevel.Fatal, invalidOperation.ToString());
                return await Task.FromResult(false);
            }
            catch (Exception ex)
            {
                Logger.Log(LogLevel.Fatal, ex.ToString());
                return await Task.FromResult(false);
            }
        }

        public static async Task<bool> BuildInstances(string apiKey)
        {
            if (instances != null)
                instances.Clear(); // safety net that should never be needed
            else
                instances = new List<Instance>();

            try
            {
                var request = WebRequest.Create("https://cn.fflogs.com:443/v1/zones?api_key=" + apiKey);
                var response = await request.GetResponseAsync();
                if (response.GetResponseStream() != null)
                    using (var reader =
                        new StreamReader(response.GetResponseStream() ?? throw new InvalidOperationException()))
                    {
                        var json = reader.ReadToEnd();
                        var array = JArray.Parse(json);
                        if (array.HasValues)
                            foreach (var category in array)
                                if (category["frozen"].ToObject<bool>() == false &&
                                    category["id"].ToObject<int>() != 2 && category["id"].ToObject<int>() != 14 && category["id"].ToObject<int>() != 22 && category["id"].ToObject<int>() != 26)
                                    foreach (var encounter in category["encounters"])
                                    {
                                        var encObj = new Encounter();
                                        encObj.Name =
                                            category["id"].ToObject<int>() == 21 || category["id"].ToObject<int>() == 25
                                                ? encounter["name"].ToObject<string>() + " (Savage)"
                                                : encounter["name"].ToObject<string>();
                                        encObj.Key = encounter["id"].ToObject<int>();
                                        encObj.Category = category["id"].ToObject<int>();

                                        var instance = instances.FirstOrDefault(i =>
                                            i.MapName == Instance.InstanceFromBoss(encObj.Name));

                                        if (instance == null)
                                        {
                                            instance = new Instance();
                                            instance.MapName = Instance.InstanceFromBoss(encObj.Name);
                                            instance.Encounters = new Dictionary<string, Encounter>();
                                            instances.Add(instance);
                                        }

                                        instance.Encounters.Add(encObj.Name, encObj);
                                        Logger.Log(LogLevel.Info,
                                            string.Format("Parsed Encounter: {0} with key {1} From Instance: {2}.", encObj.Name, encObj.Key,
                                                instance.MapName));
                                    }
                    }

                return await Task.FromResult(true);
            }
            catch (InvalidOperationException invalidOperation)
            {
                Logger.Log(LogLevel.Fatal, "Invalid Operation Occurred, Response Stream for Instances/Zones is null.");
                Logger.Log(LogLevel.Fatal, invalidOperation.ToString());
                return await Task.FromResult(false);
            }
            catch (Exception ex)
            {
                Logger.Log(LogLevel.Fatal, ex.ToString());
                return await Task.FromResult(false);
            }
        }
        

        public static async Task<bool> BuildPercentiles(string APIKey)
        {
            try
            {
                foreach (var job in jobs)
                foreach (var instance in instances)
                foreach (var enc in instance.Encounters.Values)
                {
                    if (enc.Category == 2) continue;
                    var ranking = await GetRankingData(job, enc, APIKey);
                    if (ranking == false)
                    {
                        Logger.Log(LogLevel.Error,
                            "Failed Result: " + job.Name + "  Encounter " + enc.Name);
                        return await Task.FromResult(false);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LogLevel.Error, " Failed to parse encounters ");
                return await Task.FromResult(false);
            }

            percentileData.LastUpdated = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            return await Task.FromResult(true);
        }

        public static async Task<bool> GetRankingData(ClassJob job, Encounter enc, string apiKey)
        {
            try
            {
                Logger.Log(LogLevel.Info,
                    "Starting Parse Data For: " + job.Name + " Encounter " + enc.Name);
                Logger.Log(LogLevel.Info,
                    "Job key is: " + job.Key + " Encounter key is: " + enc.Key);
                var currentTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                var startTime = percentileData.LastUpdated != 0
                    ? percentileData.LastUpdated
                    : new DateTimeOffset(DateTime.Now.Subtract(TimeSpan.FromDays(500))).ToUnixTimeMilliseconds();
                var rankings = new List<double>();
                var hasMorePages = true;
                var page = 1;
                while (hasMorePages)
                {
                    var request = WebRequest.Create(string.Format(
                        "https://cn.fflogs.com:443/v1/rankings/encounter/{0}?spec={1}&page={2}&filter=date.{3}.{4}&api_key=" +
                        apiKey,
                        enc.Key, job.Key, page, startTime, currentTime));
                    request.Timeout = 5000;

                    var response = await request.GetResponseAsync();
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        var json = reader.ReadToEnd();
                        var rankingObj = JObject.Parse(json);

                        if (rankingObj.HasValues)
                        {
                            var count = rankingObj["count"].ToObject<int>();
                            hasMorePages = rankingObj["hasMorePages"].ToObject<bool>();
                            var rankingArray = rankingObj["rankings"].ToObject<JArray>();
                            if (rankingArray.HasValues)
                                foreach (var ranking in rankingArray)
                                    rankings.Add(ranking["total"].ToObject<double>());
                        }
                    }

                    Thread.Sleep(500);
                    Logger.Log(LogLevel.Info, "Successfully Read Page: " + page);
                    ++page;
                }

                Logger.Log(LogLevel.Info, "Successfully Read " + rankings.Count + " Rankings.");


   
                var name = enc.Name.ToLower();
                switch (name)
                {
                    case "至尊巴哈姆特":
                        name = "Bahamut Prime";
                        break;
                    case "究极神兵":
                        name = "The Ultima Weapon";
                        break;
                    case "卡奥斯 (savage)":
                        name = "Chaos (Savage)";
                        break;
                    case "カオス":
                    case "卡奥斯":
                        name = "Chaos";
                        break;
                    case "尘世幻龙 (savage)":
                        name = "Midgardsormr (Savage)";
                        break;
                    case "欧米茄 (savage)":
                        name = "Omega (Savage)";
                        break;
                    case "双生欧米茄":
                        name = "Omega-M and Omega-F";
                        break;
                    case "双生欧米茄 (savage)":
                        name = "Omega-M and Omega-F (Savage)";
                        break;
                    case "至尊欧米茄 (savage)":
                        name = "The Final Omega (Savage)";
                        break;
                    case "尘世幻龙":
                    case "ミドガルズオルム":
                        name = "Midgardsormr";
                        break;
                    case "欧米茄":
                    case "オメガ":
                        name = "Omega";
                        break;
                    case "朱雀":
                        name = "Suzaku";
                        break;
                    case "ツクヨミ":
                    case "月读":
                        name = "Tsukuyomi";
                        break;
                    case "白虎":
                        name = "Byakko";
                        break;
                    case "神龙":
                    case "神龍":
                        name = "Shinryu";
                        break;
                    case "吉祥天女":
                    case "ラクシュミ":
                        name = "Lakshmi";
                        break;
                    case "须佐之男":
                    case "スサノオ":
                        name = "Susano";
                        break;
                    case "青龙":
                    case "青龍":
                        name = "Seiryu";
                        break;
                    case "暗黒の雲ファムフリート":
                    case "暗黑之云法姆弗里特":
                        name = "Famfrit, the Darkening Cloud";
                        break;
                    case "魔人ベリアス":
                    case "魔人贝利亚斯":
                        name = "Belias, the Gigas";
                        break;
                    case "労働七号":
                    case "劳动七号":
                        name = "Construct 7";
                        break;
                    case "鬼龍ヤズマット":
                    case "鬼龙雅兹玛特":
                        name = "Yiazmat";
                        break;
                }
                name = name.ToLower();
                if (percentileData.Rankings.ContainsKey(name) != true)
                {
                    percentileData.Rankings.Add(name, new Dictionary<string, List<double>>());
                }
                if (percentileData.Rankings[name].ContainsKey(job.Abbrveiation) != true)
                {
                    percentileData.Rankings[name].Add(job.Abbrveiation, new List<double>());
                }

                percentileData.Rankings[name][job.Abbrveiation].AddRange(rankings);
                Logger.Log(LogLevel.Info, "Success For Job " + job.Name + " Encounter " + enc.Name);
                Logger.Log(LogLevel.Info, "Job Abbr is " + job.Abbrveiation + " Encounter name is " + name);
                return await Task.FromResult(true);
            }
            catch (WebException ex)
            {
                Logger.Log(LogLevel.Warn, ex.Message);
                Logger.Log(LogLevel.Warn, "No Data Exists for that Encounter and Job.");
                return await Task.FromResult(true);
            }
        }
    }
}