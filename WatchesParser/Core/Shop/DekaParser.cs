using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using WatchesParser.Core.Models;

namespace WatchesParser.Core.Shop
{
    class DekaParser : IParser
    {
        public string[] ParsePage(IHtmlDocument document)
        {
            throw new NotImplementedException();
        }

        public List<ArgumentValue> ParseWatch(IHtmlDocument document)
        {
            return document.QuerySelectorAll("div").Where(item => item.ClassName != null
             && item.ClassName.Contains("bx_catalog_item_title"))
                .Select(x =>
                new ArgumentValue()
                {
                    Argument = x.Children[0].TextContent,
                    Value = x.NextElementSibling.Children[1].TextContent
                }).ToList();
        }
        
    }
}
