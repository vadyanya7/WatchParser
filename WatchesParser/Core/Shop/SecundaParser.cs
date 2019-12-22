using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp.Html.Dom;
using Newtonsoft.Json;
using WatchesParser.Core.Models;

namespace WatchesParser.Core.Shop
{
    class SecundaParser : IParser
    {
        public string[] ParsePage(IHtmlDocument document)
        {
            throw new NotImplementedException();
        }

        public List<ArgumentValue> ParseWatch(IHtmlDocument document)
        {
            var list = document.QuerySelectorAll("div").Where(item => item.ClassName != null
             && item.ClassName.Contains("catalog-box"))
               .FirstOrDefault().GetAttribute("data-product-list");

             List<Watch> listOfWatch = JsonConvert.DeserializeObject<List<Watch>>("["+list+"]");//to do
            return null;
        }
    }
}
