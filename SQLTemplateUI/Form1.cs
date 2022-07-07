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
            threads.Text = Properties.Settings.Default.Threads;
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
            if(folderPath.ShowDialog() == DialogResult.OK) 
            { 
                exactData.Text = folderPath.SelectedPath; 
                var dir = new DirectoryInfo(exactData.Text);
                double totalsize = dir.GetFiles().Sum(file => file.Length);
                totalsize = +Math.Ceiling(totalsize / 1024 / 1024 / 1024);
                outputText.Text = $"EXACTData is {totalsize}GB (rounded up)";
                dbSelect.Text = Convert.ToString(totalsize);
            }

        }
        private async void AllInOne_Click(object sender, EventArgs e)
        {
            var ErrorCheck = new ErrorChecking(slugBox.Text, sqlpathBox.Text, exactData.Text, stagingBox.Text, dbSelect.Text, SQLServer.Text);
            
            if (ErrorCheck.ErrorCheck() == 0)
            {
                return;
            }

            if (stagingBox.Text == "production")
            {
                DialogResult dialog = MessageBox.Show($"APPT6 Was Last Modified :{File.GetLastWriteTime($"{exactData.Text}\\APPT6.fs5")}, Continue?", "Continue?", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.No)
                {
                    return;
                }
            }

            string newLine = Environment.NewLine;
            var UpsizeMe = new MainProcess(slugBox.Text,sqlpathBox.Text, SQLServer.Text,stagingBox.Text, dbSelect.Text, exactData.Text, threads.Text, this);
            var ZipMe = new Zip(sqlpathBox.Text, slugBox.Text, stagingBox.Text);
            string elapsedTime = null;

            AllInOne.Enabled = false;
            button1.Enabled = false;
            Generate.Enabled = false; 
            sqlButton.Enabled = false;
            exactButton.Enabled = false;


            progressBar1.MarqueeAnimationSpeed = 10;

            await Task.Run(() =>
            {
                elapsedTime = UpsizeMe.ProcessRun();
                
            });

            progressBar1.Style = ProgressBarStyle.Blocks;
            progressBar1.MarqueeAnimationSpeed = 0;
            progressBar1.Value = 0;

            outputText.Text = $"Creating backup completed...Files output to {sqlpathBox.Text}" + newLine + "" + newLine + $"Finished in...{elapsedTime}";

            DialogResult dialogResult = MessageBox.Show($"Successfully Created Backup in {sqlpathBox.Text}, do you want to zip the backup?", "Zip backup?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                outputText.Text += newLine + "Zipping backup please wait..." + newLine + "";

                progressBar1.Style = ProgressBarStyle.Marquee;
                progressBar1.MarqueeAnimationSpeed = 10;

                await Task.Run(() =>
                {
                    ZipMe.ZipBackup();
                });

                progressBar1.Style = ProgressBarStyle.Blocks;
                progressBar1.MarqueeAnimationSpeed = 0;
                progressBar1.Value = 0;

                outputText.Text += newLine + $"File has been zipped in the following location {sqlpathBox.Text}\\{slugBox.Text}-{stagingBox.Text}.zip";
            }
            else if (dialogResult == DialogResult.No)
            {

            }

            Neymar.OpenNeymar(stagingBox.Text, slugBox.Text, siteID.Text, sqlpathBox.Text);

            progressBar1.Style = ProgressBarStyle.Marquee;

            AllInOne.Enabled = true;
            button1.Enabled = true;
            Generate.Enabled = true;
            sqlButton.Enabled = true;
            exactButton.Enabled = true;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            ServiceRestart.RestartService();
        }

        private void exactData_TextChanged(object sender, EventArgs e)
        {
            if (Directory.Exists(exactData.Text))
            {
                var dir = new DirectoryInfo(exactData.Text);
                double totalsize = dir.GetFiles().Sum(file => file.Length);
                totalsize = +Math.Ceiling(totalsize / 1024 / 1024 / 1024);
                outputText.Text = $"EXACTData is {totalsize}GB (rounded up)";
                dbSelect.Text = Convert.ToString(totalsize);
            }

        }
    }
    }


