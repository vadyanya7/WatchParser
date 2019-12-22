using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchesParser.Core;
using WatchesParser.Core.Models;

namespace WatchesParser.Shop
{
    class ShopParse : IParser
    {

        public string[] ParsePage(IHtmlDocument document)
        {
            var items = document.QuerySelectorAll("a")
             .Where(item => item.ClassName != null
             && item.ClassName.Contains("products-list-img"));

            var hrefs = items.Select(m => ((IHtmlAnchorElement)m).Href).ToArray();
            return hrefs;
        }

        public List<ArgumentValue> ParseWatch(IHtmlDocument document)
        {
            var keyValues = document.QuerySelectorAll("div")
                .Where(item => item.ClassName != null
                && (item.ClassName.Contains("sing-2 col-sm-12")
                || item.ClassName.Contains("sing-1 col-sm-12")
                ) && !item.ParentElement.PreviousElementSibling
                .TextContent.Contains("Funkcje")).ToList();

            var nazwaInterface = GetInterfaces(document);

            var funkcjes = Funciies(nazwaInterface);

            var list = GetList(keyValues);

            var nameWatch = GetNameWatch(document);

            var price = GetPrice(document);

            var imgUrls = GetImageUrls(document);

            var imgUrlValues = imgUrls.Select(x => new ArgumentValue()
            {
                Argument = "img-url-" + (Array.IndexOf(imgUrls, x) + 1).ToString(),
                Value = x
            });

            var isRed = IsAviable(document);

            var kod = GetKod(document);

            DeleteDuplicates(list);


            list.Insert(0, new ArgumentValue()
            {
                Argument = "NAme",
                Value = nameWatch
            });
            list.Insert(1, new ArgumentValue()
            {
                Argument = "Brand",
                Value = nameWatch.Split(' ').FirstOrDefault()
            });

            list.Insert(2, new ArgumentValue()
            {
                Argument = "Price",
                Value = price
            });

            list.Insert(3, new ArgumentValue()
            {
                Argument = "kod",
                Value = kod
            });

            list.Insert(4, new ArgumentValue()
            {
                Argument = "IsAviable",
                Value = !isRed ? "Aviable" : "Ban"
            });

            list.Add(new ArgumentValue()
            {
                Argument = "Funkcje",
                Value = funkcjes
            });
            list.AddRange(imgUrlValues);
            return list;
        }

        private void DeleteDuplicates(List<ArgumentValue> list)
        {
            foreach (var x in list)
            {
                if (string.IsNullOrEmpty(x.Argument))
                {
                    x.Argument = x.Value;
                }
            }
        }

        private List<IElement> GetInterfaces(IHtmlDocument document)
        {
            return document.QuerySelectorAll("div")
                .Where(item => item.ClassName != null
                && (item.ClassName.Contains("sing-2 col-sm-12")
                || item.ClassName.Contains("sing-1 col-sm-12"))
                && item.ParentElement.PreviousElementSibling
                .TextContent.Contains("Funkcje")).ToList();
        }

        private string Funciies(List<IElement> interfaces)
        {
            return string.Join(", ", interfaces.Select(item =>
                       item.Children.Where(i => i.ClassName != null && (i.ClassName.Contains("nazwa ") ||
                       i.Children.Any(j => j.ClassName.Contains("nazwa "))))
                       .Select(e => e.TextContent.Replace("\n", " ")
                       .Replace("  ", "").TrimStart()).FirstOrDefault()));
        }

        private List<ArgumentValue> GetList(List<IElement> keyValues)
        {
            return keyValues.Select(item =>
                    new ArgumentValue()
                    {
                        Value = item.Children.Where(i => i.ClassName != null && (i.ClassName.Contains("nazwa ") ||
                        i.Children.Any(j => j.ClassName.Contains("nazwa "))))
                       .Select(e => string.Join(", ", e.TextContent.Trim().Replace("  ", String.Empty).Split('\n'))).FirstOrDefault(),
                        Argument = item.Children.Where(i => i.ClassName != null && i.ClassName.Contains("nazwa_grupy "))
                      .Select(e => e.TextContent.Trim()).FirstOrDefault()
                    }
                  ).ToList();
        }

        private string GetNameWatch(IHtmlDocument document)
        {
            return document.QuerySelectorAll("h1").
                Select(item => item.TextContent.Replace("\n", " ")
               .Replace("  ", String.Empty).TrimStart()).FirstOrDefault();
        }

        private string GetPrice(IHtmlDocument document)
        {
            return document.QuerySelectorAll("span")
                .Where(item => item.ClassName != null
                && item.ClassName.Contains("productspecialprice"))
                .Select(i => i.Children[0].TextContent).FirstOrDefault() ?? "__Empty__";
        }

        private string[] GetImageUrls(IHtmlDocument document)
        {
            return document.QuerySelectorAll("a")
                 .Where(item => item.ClassName != null
                 && item.ClassName.Contains("img-gallery"))
                 .Select(m => ((IHtmlAnchorElement)m).Href)
                 .Take(4).ToArray(); 
        }

        private bool IsAviable(IHtmlDocument document)
        {
            return document.QuerySelectorAll("span")
                .Where(item => item.ClassName != null
                && item.ClassName.Contains("color-red"))
                .Select(i => i.InnerHtml.Trim()).Any();
        }

        private string GetKod(IHtmlDocument document)
        {
            return document.QuerySelectorAll("div").
                Where(item => item.ClassName != null
                && item.ClassName.Contains("product-info-header"))
                .ToArray()[0].Children[0].TextContent.Split('\n')[2].Trim();
        }

    }
}
