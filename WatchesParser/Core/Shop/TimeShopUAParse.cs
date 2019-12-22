using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using WatchesParser.Core.Models;

namespace WatchesParser.Core.Shop
{
    class TimeShopUAParse : IParser
    {
        public string[] ParsePage(IHtmlDocument document)
        {
            throw new NotImplementedException();
        }

        public List<ArgumentValue> ParseWatch(IHtmlDocument document)
        {
           return document.QuerySelectorAll("a").Where(item => item.ClassName != null
             && item.ClassName.Contains("product-title"))
                .Select(x=>
                new ArgumentValue()
                {
                    Argument= x.TextContent,
                    Value = x.ParentElement.ParentElement.NextElementSibling.Children[0].
                    Children[2].Children[1].Children[2].Children[0].Children[0].TextContent
                }).ToList();
        }
    }
}
