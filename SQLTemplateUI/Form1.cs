using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Threading;
using System.IO.Compression;
using System.ServiceProcess;


namespace SQLTemplateUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            SQLServer.Text = Properties.Settings.Default.SQLPath;
        }

        private void sqlButton_Click(object sender, EventArgs e)
        {
            var folderPath = new FolderBrowserDialog();
            DialogResult  = folderPath.ShowDialog();
            sqlpathBox.Text = folderPath.SelectedPath;
        }
        private void Generate_Click(object sender, EventArgs e)
        {
            var ErrorCheck = new ErrorChecking(slugBox.Text, sqlpathBox.Text, exactData.Text, stagingBox.Text, dbSelect.Text, SQLServer.Text);

            if (ErrorCheck.ErrorCheck() == 0)
            {
                return;
            }
            else
            {
                var template = new GenerateTemplate(slugBox.Text, sqlpathBox.Text, stagingBox.Text, dbSelect.Text);

                outputText.Text = template.TemplateGenerator();
                Clipboard.SetText(outputText.Text);
                MessageBox.Show("Ensure EXACTData has been upgraded & Exact Server has been stopped before running upsize!\n \nResults copied to clipboard!", "Reminder", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            var folderPath = new FolderBrowserDialog();
            folderPath.SelectedPath = sqlpathBox.Text;
            if(folderPath.ShowDialog() == DialogResult.OK) { exactData.Text = folderPath.SelectedPath; }
            var dir = new DirectoryInfo(exactData.Text);
            double totalsize = dir.GetFiles().Sum(file => file.Length);
            totalsize =+ Math.Ceiling(totalsize / 1024 / 1024 / 1024);
            outputText.Text = $"EXACTData is {totalsize}GB (rounded up)";
            dbSelect.Text = Convert.ToString(totalsize);
        }
        private void AllInOne_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            string newLine = Environment.NewLine;
            Stopwatch stopWatch = new Stopwatch();
            var UpsizeData = new SQL(slugBox.Text, sqlpathBox.Text, SQLServer.Text, stagingBox.Text, dbSelect.Text, exactData.Text, threads.Text);
            var ErrorCheck = new ErrorChecking(slugBox.Text, sqlpathBox.Text, exactData.Text, stagingBox.Text, dbSelect.Text, SQLServer.Text);

            if (ErrorCheck.ErrorCheck() == 0)
            {
                return;
            }

            stopWatch.Start();
            outputText.Text = "CREATING DB PLEASE WAIT....";

            Task CreateDB = Task.Run(() =>
            {
                UpsizeData.CreateDb();
            });

            progressBar1.Increment(25);
            CreateDB.Wait();
            UpsizeData.RestoreDb();
            progressBar1.Increment(25);
            outputText.Text = "RUNNING UPSIZE PLEASE WAIT....";
            UpsizeData.RunUpsize();
            while  (Process.GetProcessesByName("parallelupsizer").Length > 0) { }
            progressBar1.Increment(25);
            outputText.Text = "CREATING BACKUP PLEASE WAIT....";
            Task BDB = Task.Run(() =>
            {
                UpsizeData.BackupDb();
            });
            BDB.Wait();
            UpsizeData.DetachDb();
            progressBar1.Increment(25);
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
            outputText.Text = $"Creating backup completed...Files output to {sqlpathBox.Text}"+newLine+""+newLine+$"Finished in...{elapsedTime}";
            Properties.Settings.Default.SQLPath = SQLServer.Text;
            Properties.Settings.Default.Save();
            progressBar1.Value = 0;
            
            DialogResult dialogResult = MessageBox.Show($"Successfully Created Backup in {sqlpathBox.Text}, do you want to zip the backup?", "Zip backup?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                outputText.Text += newLine + "Zipping backup please wait..."+newLine+"";
                progressBar1.Increment(50);
                Zip.ZipBackup(sqlpathBox.Text, slugBox.Text, stagingBox.Text);
                progressBar1.Increment(50);
                outputText.Text += newLine + $"File has been zipped in the following location {sqlpathBox.Text}\\{slugBox.Text}-{stagingBox.Text}.zip";
            }
            else if (dialogResult == DialogResult.No)
            {

            }

            Neymar.OpenNeymar(stagingBox.Text, slugBox.Text, siteID.Text, sqlpathBox.Text);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            ServiceRestart.RestartService();
        }
    }
    }


