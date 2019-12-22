using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchesParser.Core.Shop
{
    class SecundaSetting : IParserSettings
    {
        public SecundaSetting(int start, int end)
        {
            StartPoint = start;
            EndPoint = end;
        }
        public string BaseUrl { get; set; } = "https://secunda.com.ua/shop/chasy-naruchnye";
        public int StartPoint { get; set; }
        public int EndPoint { get; set; }
    }
}
