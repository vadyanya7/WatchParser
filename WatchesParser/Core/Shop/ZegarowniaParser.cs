using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using WatchesParser.Core.Models;

namespace WatchesParser.Core.Shop
{
    class ZegarowniaParser : IParser
    {
        public string[] ParsePage(IHtmlDocument document)
        {
            var items = document.QuerySelectorAll("h2")
             .Where(item => item.ClassName != null
             && item.ClassName.Contains("product-name"));

            var hrefs = items.Select(m => ((IHtmlAnchorElement)m.NextElementSibling)
            .Href.Replace("about://", "https://")).ToArray();
            return hrefs;
        }

        public List<ArgumentValue> ParseWatch(IHtmlDocument document)
        {
            var list = new List<ArgumentValue>();

            var name = GetName(document);
            var price = GetPrice(document);
            var arguments = GetArguments(document);
            var imges = GetImagesUrls(document); 

            var imgUrlValues = imges.Select(x => new ArgumentValue()
            {
                Argument = "img-url-" + (Array.IndexOf(imges.ToArray(), x) + 1).ToString(),
                Value = x
            });

            list.Insert(0, new ArgumentValue()
            {
                Argument = "Price",
                Value = price
            });

            list.Insert(1, new ArgumentValue()
            {
                Argument = "Name",
                Value = name
            });

            list.AddRange(arguments);
            list.AddRange(imgUrlValues);
            return list;
        }

        private string GetName(IHtmlDocument document)
        {
            return document.QuerySelectorAll("h1")
                .Where(item => item.ClassName != null
                && item.ClassName.Contains("product-title__heading"))
                .Select(i => i.TextContent.Trim().Replace("  ", String.Empty)).FirstOrDefault() ?? "__empty__";
        }
        private string GetPrice(IHtmlDocument document)
        { 
            return document.QuerySelectorAll("span").
                Where(x => x.ClassName != null && x.ClassName.Contains("price")
                && !x.ClassName.Contains("price-label"))
               .Select(item => item.TextContent.Replace(",00 zł", "")
               .Replace(" ", String.Empty).TrimStart()).ToArray()[1];
        }
        private List<ArgumentValue> GetArguments(IHtmlDocument document)
        {
            return document.QuerySelectorAll("div").
                Where(x => x.ClassName != null && x.ClassName.Contains("data-list__item-inner"))
                .Select(x => new ArgumentValue()
                {
                    Argument = x.Children[0].TextContent.Trim().Replace("  ", ""),
                    Value = x.Children[1].TextContent.Trim().Replace("  ", "")
                }).ToList();
        }
        private List<string> GetImagesUrls(IHtmlDocument document)
        {
            return document.QuerySelectorAll("button")
                .Where(x => x.ClassName != null && x.ClassName.Contains("zoom-gallery__thumb-link--image"))
                .Select(x => x.GetAttribute("data-image-zoom")).ToList().Take(4).ToList();
        }
    }
}
