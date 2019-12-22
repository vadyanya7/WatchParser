using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchesParser.Core;

namespace WatchesParser.Shop
{
    class ShopSettings : IParserSettings
    {
        public ShopSettings(int start,int end)
        {
            StartPoint = start;
            EndPoint = end;
        }
        public string BaseUrl { get; set ; } = "https://www.zegarek.net/zegarki.html?page=";
        public int StartPoint { get ; set ; }
        public int EndPoint { get ; set ; }
    }
}
