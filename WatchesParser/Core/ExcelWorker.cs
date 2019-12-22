using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using WatchesParser.Core.Models;

namespace WatchesParser.Core
{
    public class ExcelWorker
    {

        private static List<string> _argumentNames = new List<string>();

        public void AddCells(ExcelPackage excel, List<ArgumentValue> headerRow, int count)
        {
            var newsArguments = headerRow.Select(x => x.Argument).Except(_argumentNames).ToList();

            _argumentNames.AddRange(newsArguments);
            List<string[]> list = new List<string[]>();
            list.Add(headerRow.Select(i => i.Value).ToArray());
            var worksheet = excel.Workbook.Worksheets["Worksheet1"];

            var ColumnNamesArrays = _argumentNames.ToArray();
            for (int x = 1; x < _argumentNames.Count; x++)
            {
                worksheet.Cells[1, x].Value = ColumnNamesArrays[x - 1];
            }

            int Row = 1;
            foreach (var x in headerRow)
            {
                var row = _argumentNames.FindIndex(g => g == x.Argument) + 1;
                worksheet.Cells[count, row].Value = x.Value;
                Row++;
            }
            count++;
        }

        public void GetDuplicates()
        {
            var excelFile = new FileInfo(@"C:\Users\Vadim\Desktop\Дубликаты немцы. поляки.xlsx");
            var excel = new ExcelPackage(new FileInfo(@"C:\Users\Vadim\Desktop\Дубликаты немцы. поляки.xlsx"));
            var worksheet3 = excel.Workbook.Worksheets["Дубли"];
            var polands = GetWatches(excel.Workbook.Worksheets[1]);
            var germans = GetWatches(excel.Workbook.Worksheets[2]);

            var unionEan = polands.Select(x => x.Ean).Intersect(germans.Select(x => x.Ean)).ToList();

            int cell = 2;
            foreach (var ean in unionEan)
            {
                var watch = germans.FirstOrDefault(x => x.Ean == ean);
                float priceP;
                float priceG;
                float.TryParse(polands.FirstOrDefault(x => x.Ean == ean).Price, out priceP);
                float.TryParse(germans.FirstOrDefault(x => x.Ean == ean).Price, out priceG);

                var f = float.Parse(polands.FirstOrDefault(x => x.Ean == ean).Price);

                var row = priceP < priceG ? 4 : 5;
                worksheet3.Cells[cell, 1].Value = watch.Ean;
                worksheet3.Cells[cell, 2].Value = watch.Name;
                worksheet3.Cells[cell, 3].Value = watch.Brand;
                worksheet3.Cells[cell, 4].Value = priceP;
                worksheet3.Cells[cell, 5].Value = priceG;
                worksheet3.Cells[cell, row].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet3.Cells[cell, row].Style.Fill.BackgroundColor.SetColor(Color.YellowGreen);
                cell++;
            }
            excel.SaveAs(excelFile);
        }

        private List<WatchModel4> GetWatches(ExcelWorksheet sheet1)
        {
            var list = new List<WatchModel4>();

            for (int i = 3; ; i++)
            {
                var name = sheet1.Cells[i, 3].Text;
                if (string.IsNullOrEmpty(sheet1.Cells[i, 1].Text))
                {
                    break;
                }
                list.Add(new WatchModel4()
                {
                    Ean = name,
                    Name = sheet1.Cells[i, 2].Text,
                    Brand = sheet1.Cells[i, 3].Text,
                    Price = sheet1.Cells[i, 4].Text
                });
            }
            return list;
        }

        private List<WatchModel4> GetWatches(ExcelWorksheet sheet1, int row)
        {
            var list = new List<WatchModel4>();

            for (int i = 2; ; i++)
            {
                var name = sheet1.Cells[i, 3].Text;
                if (string.IsNullOrEmpty(sheet1.Cells[i, row].Text))
                {
                    break;
                }
                list.Add(new WatchModel4()
                {
                    Ean = name,
                    Name = sheet1.Cells[i, 2].Text,
                    Brand = sheet1.Cells[i, 3].Text,
                    Price = sheet1.Cells[i, 4].Text
                });
            }
            return list;
        }

        public void PaintDuplicate()
        {
            var excelFile = new FileInfo(@"C:\Users\Vadim\Desktop\Zegarki. net. Парсинг 12.12.2019.xlsx");
            var excel = new ExcelPackage(excelFile);
            var worksheet1 = excel.Workbook.Worksheets[1];
            var watches = GetWatches(worksheet1);
            var eans = GetWatches(worksheet1)
                .Where(x => !string.IsNullOrEmpty(x.Name)).Select(x => x.Name).ToList();
            var query = eans.GroupBy(x => x)
               .Where(g => g.Count() > 1)
               .Select(y => y.Key)
               .ToList();
            int cell = 3;
            foreach (var x in watches)
            {
                var item = query.FirstOrDefault(it => it == x.Name);
                if (item != null && item.Contains(x.Name))
                {
                    worksheet1.Cells[cell, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet1.Cells[cell, 2].Style.Fill.BackgroundColor.SetColor(Color.OrangeRed);
                }
                cell++;
            }
            excel.SaveAs(excelFile);
        }

        public void PaintUnion()
        {
            var excelFileZegarki = new FileInfo(@"C:\Users\Vadim\Desktop\Zegarki. net. Парсинг 12.12.2019_2 — копия.xlsx");
            var excelZegarki = new ExcelPackage(excelFileZegarki);
            var excelFileUnion = new FileInfo(@"C:\Users\Vadim\Desktop\Дубликаты немцы. поляки.xlsx");
            var excelUnion = new ExcelPackage(excelFileUnion);
            var worksheet1 = excelZegarki.Workbook.Worksheets[1];
            var zegarkiEans = GetWatches(worksheet1).Select(x => x.Name).ToList();

            var unionEans = GetWatches(excelUnion.Workbook.Worksheets[3])
                .Where(x => !string.IsNullOrEmpty(x.Name))
                .Select(x => x.Ean).ToList();

            int cell = 3;
            foreach (var eanZ in zegarkiEans)
            {
                if (unionEans.FirstOrDefault(x => x.Contains(eanZ)) != null)
                {

                    if (!string.IsNullOrEmpty(eanZ))
                    {
                        worksheet1.Cells[cell, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet1.Cells[cell, 3].Style.Fill.BackgroundColor.SetColor(Color.Aqua);
                    }
                    worksheet1.Cells[cell, 3].Value = eanZ;
                }
                cell++;
            }
            excelZegarki.SaveAs(excelFileZegarki);
        }

        public void GenerateDeku(ExcelPackage excel, List<ArgumentValue> listOfWatch, int count)
        {
            var worksheet = excel.Workbook.Worksheets["Deku"];
            foreach (var x in listOfWatch)
            {
                worksheet.Cells[count, 1].Value = x.Argument;
                worksheet.Cells[count, 2].Value = x.Argument.Split(' ').LastOrDefault();
                worksheet.Cells[count, 3].Value = x.Value;
                count++;
            }
        }

        public void MinimumPriceOf3Items()
        {
            var excelFile = new FileInfo(@"C:\Users\Vadim\Desktop\Сводный файл с цен конкурентов.xlsx");
            var excelTimes = new ExcelPackage(excelFile);
            var ourWatches = GetWatches(excelTimes.Workbook.Worksheets[1])
                .Select(x => x.Name).ToList();
            var worksheet1 = excelTimes.Workbook.Worksheets[1];
            int cell = 3;

            var timeShopList = GetWatches(excelTimes.Workbook.Worksheets[2])
                    .ToList();
            var secundaList = GetWatches(excelTimes.Workbook.Worksheets[3])
                .ToList();
            var dekaList = GetWatches(excelTimes.Workbook.Worksheets[4])
               .ToList();

            foreach (var x in ourWatches)
            {
                var timeShopEans = timeShopList.Where(it => it.Ean.Contains(x)).ToList();
                var secundaEans = secundaList.Where(it => it.Ean.Contains(x)).ToList();
                var dekaEans = dekaList.Where(it => it.Ean.Contains(x)).ToList();
                var timeShopPrice = timeShopEans.Select(item => item.Brand).FirstOrDefault();
                var secundaPrice = secundaEans.Select(item => item.Brand).FirstOrDefault();
                var dekaPrice = dekaEans.Select(item => item.Brand).FirstOrDefault();
 
                if (timeShopEans.Count > 1 || secundaEans.Count > 1 || dekaEans.Count > 1)
                {
                    string text = timeShopEans.Count > 1 ? "timeShop" : String.Empty;
                    text += secundaEans.Count > 1 ? " + secunda +" : String.Empty;
                    text += dekaList.Count > 1 ? " + deka" : String.Empty;
                    worksheet1.Cells[cell, 12].Value = text;
                    cell++;
                    continue;
                }
                var prices = ToIntArray(timeShopPrice, secundaPrice, dekaPrice);

                worksheet1.Cells[cell, 8].Value = timeShopPrice;
                worksheet1.Cells[cell, 9].Value = secundaPrice;
                worksheet1.Cells[cell, 10].Value = dekaPrice;
                for (int i = 8; i <= 10; i++)
                {
                    if (Convert.ToInt32(worksheet1.Cells[cell, i].Value) == prices.Min())
                    {
                        worksheet1.Cells[cell, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet1.Cells[cell, i].Style.Fill.BackgroundColor.SetColor(Color.Aqua);
                        worksheet1.Cells[cell, 11].Value = prices.Min();
                    }
                }
                cell++;
            }
            excelTimes.SaveAs(excelFile);
        }

        private int[] ToIntArray(string number1, string number2, string number3)
        {
            int c = Int32.TryParse(number1, out c) ? c  : Int32.MaxValue;
            int d = Int32.TryParse(number2, out d) ? d : Int32.MaxValue;
            int e = Int32.TryParse(number3, out e) ? e : Int32.MaxValue;
            int[] arr = new [] { c , d , e};         
            return arr;
        }

        public void PaintDistinct()
        {
            var excelFile = new FileInfo(@"C:\Users\Vadim\Desktop\ZegarowniaПарсинг16.12.2019(Полный).xlsx");
            var excel = new ExcelPackage(excelFile);
            var worksheet1 = excel.Workbook.Worksheets[1];
            var zegarowniaList = GetWatches(worksheet1, 2).Select(x => x.Name);
            var zegarkiList = GetWatches(worksheet1, 3).Select(x => x.Brand);
            var distinctList = zegarowniaList.Intersect(zegarkiList).ToList();
            int cell2 = 1;

            foreach (var dist in zegarowniaList)
            {
                cell2++;
                var item = distinctList.FirstOrDefault(x => x == dist);
                if (item != null)
                {
                    worksheet1.Cells[cell2, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet1.Cells[cell2, 2].Style.Fill.BackgroundColor.SetColor(Color.OrangeRed);
                }
            }
            int cell3 = 1;
            foreach (var dist in zegarkiList)
            {
                cell3++;
                var item = distinctList.FirstOrDefault(x => x == dist);
                if (item != null)
                {
                    worksheet1.Cells[cell3, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet1.Cells[cell3, 3].Style.Fill.BackgroundColor.SetColor(Color.OrangeRed);
                }
            }
            excel.SaveAs(excelFile);
        }
    }
}
