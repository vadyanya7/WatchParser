using AngleSharp.Html.Dom;
using System.Collections.Generic;
using WatchesParser.Core.Models;

namespace WatchesParser.Core
{
    interface IParser
    {
        string[] ParsePage(IHtmlDocument document);
        List<ArgumentValue> ParseWatch(IHtmlDocument document);
    }
}
