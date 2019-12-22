using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchesParser.Core.Shop
{
    class DekaSettings : IParserSettings
    {
        public DekaSettings(int start, int end)
        {
            StartPoint = start;
            EndPoint = end;
        }
        public string BaseUrl { get; set; } = "https://deka.ua/naruchnye-chasy";
        public int StartPoint { get; set; }
        public int EndPoint { get; set; }
    }
}
