
namespace WatchesParser.Core
{
    interface IParserSettings
    {
        string BaseUrl { get; set; }
        int StartPoint { get; set; }
        int EndPoint { get; set; }
    }
}
