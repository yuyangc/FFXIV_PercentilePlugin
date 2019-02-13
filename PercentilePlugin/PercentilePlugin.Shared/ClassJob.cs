using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PercentilePlugin.Shared
{
    public class ClassJob
    {
        public string Name { get; set; }
        public string Abbrveiation { get; set; }
        public int Key { get; set; }
        
        public static string NameToAbbr(string name)
        {
            switch (name)
            {
                case "Astrologian":
                case "占星术士":
                    return "AST";
                case "Bard":
                case "诗人":
                    return "BRD";
                case "Black Mage":
                case "黑魔法师":
                    return "BLM";
                case "Dark Knight":
                case "暗黑骑士":
                    return "DRK";
                case "Dragoon":
                case "龙骑士":
                    return "DRG";
                case "Machinist":
                case "机工士":
                    return "MCH";
                case "Monk":
                case "武僧":
                    return "MNK";
                case "Ninja":
                case "忍者":
                    return "NIN";
                case "Paladin":
                case "骑士":
                    return "PLD";
                case "Scholar":
                case "学者":
                    return "SCH";
                case "Summoner":
                case "召唤师":
                    return "SMN";
                case "Warrior":
                case "战士":
                    return "WAR";
                case "White Mage":
                case "白魔法师":
                    return "WHM";
                case "Red Mage":
                case "赤魔法师":
                    return "RDM";
                case "Samurai":
                case "武士":
                    return "SAM";
                default:
                    return "???";
            }
        }
    }
}
