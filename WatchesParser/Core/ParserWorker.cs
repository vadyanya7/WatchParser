using AngleSharp.Html.Parser;
using OfficeOpenXml;
using System;
using System.IO;
using WatchesParser.Core.Models;

namespace WatchesParser.Core
{
    class ParserWorker<T> where T : class
    {
        IParser parser;
        IParserSettings parserSettings;
        HtmlLoader loader;
        private string _url;
        bool isActive;

        #region Properties

        public IParser Parser
        {
            get
            {
                return parser;
            }
            set
            {
                parser = value;
            }
        }

        public IParserSettings Settings
        {
            get
            {
                return parserSettings;
            }
            set
            {
                parserSettings = value;
                loader = new HtmlLoader(value);
                _url = loader.url;
            }
        }

        public bool IsActive
        {
            get
            {
                return isActive;
            }
        }

        #endregion
        public event Action<object> OnCompleted;

        public event Action<object> OnNew;

        public ParserWorker()
        {

        }     
        public void Start()
        {
            isActive = true;
            WorkerFull();
        }

        public void StartTimeShop()
        {
            isActive = true;
            Worker("TimeShop");
        }

        public void StartSecunda()
        {
            isActive = true;
            Worker("Secunda");
        }
        public void StartDeka()
        {
            isActive = true;
            Worker("Deka");
        }
        public void Abort()
        {
            isActive = false;
        }
        private async void WorkerFull()
        {
            using (ExcelPackage excel = new ExcelPackage())
            {
                excel.Workbook.Worksheets.Add("Worksheet1");
                var ExcelWorker = new ExcelWorker();
                int count = 2;
                for (int i = parserSettings.StartPoint; i <= parserSettings.EndPoint; i++)
                {
                    if (!isActive)
                    {
                        OnCompleted?.Invoke(this);
                        return;
                    }

                    var domParser = new HtmlParser();
                    var sourcePage = await loader.GetSourceAsync(_url+ i.ToString());
                    var documentOfPage = await domParser.ParseDocumentAsync(sourcePage);

                    var resultUrls = parser.ParsePage(documentOfPage);

                    if (resultUrls != null)
                    {
                        foreach (var url in resultUrls)
                        {
                            var sourceWatch = await loader.GetSourceAsync(url);
                            var documentOfWatch = await domParser.ParseDocumentAsync(sourceWatch);
                            ExcelWorker.AddCells(excel, parser.ParseWatch(documentOfWatch), count);
                        }
                    }
                    OnNew?.Invoke(this);
                }
                FileInfo excelFile = new FileInfo(@"C:\Users\Vadim\Desktop\Zegarownia_Meskie.xlsx");
                excel.SaveAs(excelFile);
                OnCompleted?.Invoke(this);
                isActive = false;
            }
        }

        private async void Worker(string nameShop)
        {
            using (ExcelPackage excel = new ExcelPackage())
            {

                excel.Workbook.Worksheets.Add(nameShop);
                var ExcelWorker = new ExcelWorker();
                int count = 2;
                for (int i = parserSettings.StartPoint; i <= parserSettings.EndPoint; i++)
                {
                    if (!isActive)
                    {
                        OnCompleted?.Invoke(this);
                        return;
                    }

                    var sourcePage = await loader.GetSourceAsync(_url + i.ToString());
                    var domParser = new HtmlParser();

                    var documentOfPage = await domParser.ParseDocumentAsync(sourcePage);
                    var test = parser.ParseWatch(documentOfPage);
                    ExcelWorker.GenerateDeku(excel, parser.ParseWatch(documentOfPage), count);
                    OnNew?.Invoke(this);
                }
                FileInfo excelFile = new FileInfo(@"C:\Users\Vadim\Desktop\TimeSSS.xlsx");
                excel.SaveAs(excelFile);
                OnCompleted?.Invoke(this);
                isActive = false;
            }
        }

    }
}
