using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SQLTemplateUI
{
    public class MainProcess
    {
        public string SlugName { get; set; }
        public string SQLPath { get; set; }
        public string ExactData { get; set; }
        public string SQLServer { get; set; }
        public string stagingProduction { get; set; }
        public string dbSize { get; set; }
        public string Threads { get; set; }
        public Form1 form1 { get; set; }
        public MainProcess(string SlugName, string SQLPath, string SQLServer, string stagingProduction, string dbSize, string ExactData, string Threads, Form1 Form1)
        {
            this.SlugName = SlugName;
            this.SQLPath = SQLPath;
            this.ExactData = ExactData;
            this.SQLServer = SQLServer;
            this.stagingProduction = stagingProduction;
            this.dbSize = dbSize;
            this.Threads = Threads;
            this.form1 = Form1;
        }
        public string ProcessRun()
        {

            string newLine = Environment.NewLine;
            Stopwatch stopWatch = new Stopwatch();
            var UpsizeData = new SQL(SlugName, SQLPath, SQLServer, stagingProduction, dbSize, ExactData, Threads);


            

            stopWatch.Start();
            WriteTextSafe("Creating DB Please Wait....");
            UpsizeData.CreateDb();
            WriteTextSafe("Restoring DB Please Wait....");
            UpsizeData.RestoreDb();
            WriteTextSafe("Running Upsize Please Wait....");
            UpsizeData.RunUpsize();

            while (Process.GetProcessesByName("parallelupsizer").Length > 0) { }
            WriteTextSafe("Backing Up DB Please Wait....");
            UpsizeData.BackupDb();
            WriteTextSafe("Detaching DB Please Wait....");
            UpsizeData.DetachDb();

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);

            Properties.Settings.Default.SQLPath = SQLServer;
            Properties.Settings.Default.Threads = Threads;
            Properties.Settings.Default.Save();

            return elapsedTime;
        }

        public void WriteTextSafe(string text)
        {
            if (form1.outputText.InvokeRequired)
            {
                Action safeWrite = delegate { WriteTextSafe($"{text}"); };
                form1.outputText.Invoke(safeWrite);
            }
            else
                form1.outputText.Text = text;
        }
    }
}
