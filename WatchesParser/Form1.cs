using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WatchesParser.Core;
using WatchesParser.Core.Models;
using WatchesParser.Core.Shop;
using WatchesParser.Shop;

namespace WatchesParser
{
    public partial class Form1 : Form
    {
        ParserWorker<List<ArgumentValue>> parser;
        ExcelWorker excelWorker;
        public Form1()
        {
            InitializeComponent();

            parser = new ParserWorker<List<ArgumentValue>>();
            excelWorker = new ExcelWorker();
            parser.OnCompleted += Parser_OnCompleted;
            parser.OnNew += Parser_NewObject;
            progressBar1.Step = 1;
            progressBar1.Minimum = 1;
            progressBar1.Maximum = 53;
        }
     
        private void Parser_NewObject(object obj)
        {
            progressBar1.PerformStep();
        }
        private void Parser_OnCompleted(object obj)
        {
            MessageBox.Show("All works done! ");
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            parser.Settings = new ShopSettings(1, 228);
            ShopParse shopParse = new ShopParse();
            parser.Parser = shopParse;
            parser.Start();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            excelWorker.GetDuplicates();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            excelWorker.PaintDuplicate();
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            excelWorker.PaintUnion();
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            parser.Settings = new TimeShopUASetting(1, 1282);
            parser.Parser = new TimeShopUAParse();
            parser.StartTimeShop();
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            parser.Settings = new SecundaSetting(1, 447);
            parser.Parser = new SecundaParser();
            parser.StartSecunda();
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            parser.Settings = new DekaSettings(811, 1187);
            parser.Parser = new DekaParser();
            parser.StartDeka();
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            parser.Settings = new ZegarowniaSetting(1,53);
            parser.Parser = new ZegarowniaParser();
            parser.Start();
        }

        private void Button9_Click(object sender, EventArgs e)
        {
            excelWorker.MinimumPriceOf3Items();
        }

        private void Button10_Click(object sender, EventArgs e)
        {
            excelWorker.PaintDistinct();
        }
    }
}
