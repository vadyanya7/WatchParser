

namespace WatchesParser.Core.Shop
{
    class ZegarowniaSetting : IParserSettings
    {
        public ZegarowniaSetting(int start, int end)
        {
            StartPoint = start;
            EndPoint = end;
        }
        public string BaseUrl { get; set; } = "https://zegarownia.pl/zegarki-meskie?p=";
        public int StartPoint { get; set; }
        public int EndPoint { get; set; }
    }
}
