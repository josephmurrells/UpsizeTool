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
        public MainProcess(string SlugName, string SQLPath, string SQLServer, string stagingProduction, string dbSize, string ExactData, string Threads)
        {
            this.SlugName = SlugName;
            this.SQLPath = SQLPath;
            this.ExactData = ExactData;
            this.SQLServer = SQLServer;
            this.stagingProduction = stagingProduction;
            this.dbSize = dbSize;
            this.Threads = Threads;
        }
        public string ProcessRun()
        {

            string newLine = Environment.NewLine;
            Stopwatch stopWatch = new Stopwatch();
            var UpsizeData = new SQL(SlugName, SQLPath, SQLServer, stagingProduction, dbSize, ExactData, Threads);

            stopWatch.Start();

            UpsizeData.CreateDb();

            UpsizeData.RestoreDb();

            UpsizeData.RunUpsize();

            while (Process.GetProcessesByName("parallelupsizer").Length > 0) { }

            UpsizeData.BackupDb();

            UpsizeData.DetachDb();

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);

            Properties.Settings.Default.SQLPath = SQLServer;
            Properties.Settings.Default.Save();

            return elapsedTime;
        }
    }
}
