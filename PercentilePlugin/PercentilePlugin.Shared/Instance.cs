using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PercentilePlugin.Shared
{
   public class Instance
    {
        public Dictionary<string, Encounter> Encounters { get; set; } = new Dictionary<string, Encounter>();

        public string MapName { get; set; } = "";
        
        public static string InstanceFromBoss(string boss)
        {
            switch (boss)
            {
                case "Famfrit, the Darkening Cloud":
                case "暗黑之云法姆弗里特":
                case "Belias, the Gigas":
                case "魔人贝利亚斯":
                case "Construct 7":
                case "劳动七号":
                case "Yiazmat":
                case "鬼龙雅兹玛特":
                    return "The Ridorana Lighthouse";
                    break;
                case "Susano":
                case "须佐之男":
                    return "The Pool of Tribute (Extreme)";
                    break;
                case "Lakshmi":
                case "吉祥天女":
                    return "Emanation (Extreme)";
                    break;
                case "Shinryu":
                case "神龙":
                    return "The Minstrel's Ballad: Shinryu's Domain";
                    break;
                case "Byakko":
                case "白虎":
                    return "The Jade Stoa (Extreme)";
                    break;
                case "Tsukuyomi":
                case "月读":
                    return "The Minstrel's Ballad: Tsukuyomi's Pain";
                    break;
                case "Suzaku":
                case "朱雀":
                    return "Hell's Kier (Extreme)";
                    break;
                case "Seiryu":
                case "青龙":
                    return "The Wreath of Snakes (Extreme)";
                    break;
                case "Bahamut Prime":
                case "至尊巴哈姆特":
                    return "The Unending Coil of Bahamut";
                    break;
                case "The Ultima Weapon":
                case "究极神兵":
                    return "Ultimacy";
                    break;
                case "Chaos":
                case "卡奥斯":
                    return "Alphascape (V1.0)";
                    break;
                case "Midgardsormr":
                case "尘世幻龙":
                    return "Alphascape (V2.0)";
                    break;
                case "Omega":
                case "欧米茄":
                    return "Alphascape (V3.0)";
                    break;
                case "Omega-M and Omega-F":
                case "双生欧米茄":
                    return "Alphascape (V4.0)";
                    break;
                case "Omega-M and Omega-F (Savage)":
                case "双生欧米茄 (Savage)":
                case "The Final Omega (Savage)":
                case "至尊欧米茄 (Savage)":
                    return "Alphascape V4.0 (Savage)";
                    break;
                case "Chaos (Savage)":
                case "卡奥斯 (Savage)":
                    return "Alphascape V1.0 (Savage)";
                    break;
                case "Midgardsormr (Savage)":
                case "尘世幻龙 (Savage)":
                    return "Alphascape V2.0 (Savage)";
                    break;
                case "Omega (Savage)":
                case "欧米茄 (Savage)":
                    return "Alphascape V3.0 (Savage)";
                    break;
            }

            return "";
        }
    }
}
