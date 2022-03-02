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
        //Method to ensure inputs are valid & EXACTDB not running
        private Int16 ErrorCheck()
        {
            Process[] processes = Process.GetProcessesByName("SOEDBService");
            if (slugBox.Text == "")
            {
                MessageBox.Show("Slug name is blank", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
            if (!Directory.Exists(sqlpathBox.Text))
            {
                MessageBox.Show("SQL path doesn't exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
            if (!Directory.Exists(exactData.Text))
            {
                MessageBox.Show("EXACTData path doesn't exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
            if (stagingBox.Text == "")
            {
                MessageBox.Show("Please select environment", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
            if (dbSelect.Text == "0")
            {
                MessageBox.Show("Please enter valid DB size", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
            if (processes.Length > 0)
            {
                MessageBox.Show("EXACT DB still running, stop before continuing!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return 0;
            }
            if (SQLServer.Text == "") 
            {
                MessageBox.Show("Please enter SQL server!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return 0;
            }
            else
            {
                return 1;
            }
        }
        public Form1()
        {
            InitializeComponent();
            SQLServer.Text = Properties.Settings.Default.SQLPath;
            //SQLServer.Text = "SGBGILSOECONV02";
        }
        //method which runs SQL query to create new blank DB using variables entered by user
        private void CreateDbm(string SlugName, string SQLPath, string stagingProduction, string dbSize, string SQLServer)
        {
          
            {
                String str;
                SqlConnection myConn = new SqlConnection($"Server={SQLServer};Integrated security=SSPI;database=master");

                str = $"CREATE DATABASE [{SlugName}-{stagingProduction}] ON" +
                        $"(NAME = N'{SlugName}-{stagingProduction}', " +
                        $"FILENAME = N'{SQLPath}\\{SlugName}-{stagingProduction}.mdf', " +
                        $"SIZE = {dbSize}GB)" +
                        $"LOG ON(NAME = N'{SlugName}-{stagingProduction}_Log', " +
                        $"FILENAME = N'{SQLPath}\\{SlugName}-{stagingProduction}_Log.ldf', " +
                        "SIZE = 500MB)";

                SqlCommand myCommand = new SqlCommand(str, myConn);
                myCommand.CommandTimeout = 600;
                try
                {
                    myConn.Open();
                    myCommand.ExecuteNonQuery();
                    //MessageBox.Show("Database Created Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //outputText.Text = $"Step 1: Create DB completed...Files output to {SQLPath}" + Environment.NewLine;
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Environment.Exit(1);
                }
                finally
                {
                    if (myConn.State == ConnectionState.Open)
                    {
                        myConn.Close();
                    }
                }
            }
        }
        //method which runs sql query to set db recovery mode to simple
        private void RestoreDbm(string SlugName, string stagingProduction, string SQLServer)
        {
           
            {
                String str;
                SqlConnection myConn = new SqlConnection($"Server={SQLServer};Integrated security=SSPI;database=master");

                str = $"ALTER DATABASE [{SlugName}-{stagingProduction}] SET RECOVERY SIMPLE";


                SqlCommand myCommand = new SqlCommand(str, myConn);
                myCommand.CommandTimeout = 600;
                try
                {
                    myConn.Open();
                    myCommand.ExecuteNonQuery();
                    //MessageBox.Show("Recovery set to simple", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //outputText.Text += "Step 2: Set recovery mode to simple completed" + Environment.NewLine;
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                finally
                {
                    if (myConn.State == ConnectionState.Open)
                    {
                        myConn.Close();
                    }
                }
            }
        }
        //method which edits the upsizerinput.json file to insert user entered variables and then run upsize batch file
        private void RunUpsize()
        {
            {
                string upsizePath = Environment.CurrentDirectory;
                string slugName = slugBox.Text;
                string stagingProduction = stagingBox.Text;
                string json = File.ReadAllText($"{upsizePath}\\ParallelUpsizer\\upsizerinput.json");
                dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                jsonObj["SourceExactDataPath"] = exactData.Text;
                jsonObj["DestinationSqlUpsizedDbName"] = $"{slugName}-{stagingProduction}";
                jsonObj["DestinationSqlServerInstance"] = SQLServer.Text;
                jsonObj["Parallel"] = Convert.ToInt32(threads.Text);
                string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText($"{upsizePath}\\ParallelUpsizer\\upsizerinput.json", output);


                System.Diagnostics.Process.Start($"{upsizePath}\\ParallelUpsizer\\parallelupsizer.exe", $"{upsizePath}\\ParallelUpsizer\\upsizerInput.json");
                //outputText.Text += "Step 3: Run upsize completed" + Environment.NewLine;
            }
        }
        //method which runs sql query to create backup of database created earlier
        private void BackupDBm(string SlugName, string SQLPath, string stagingProduction, string SQLServer)
        {
            { 
                String str;
                String SlugName1 = $"\"{SlugName}-{stagingProduction}\"";
                SqlConnection myConn = new SqlConnection($"Server={SQLServer};Integrated security=SSPI;database=master");

                str = $"BACKUP DATABASE {SlugName1}" +
                    $"TO DISK = '{SQLPath}\\{SlugName}-{stagingProduction}.bak'" +
                    "WITH STATS";

                SqlCommand myCommand = new SqlCommand(str, myConn);
                myCommand.CommandTimeout = 600;
                try
                {
                    myConn.Open();
                    myCommand.ExecuteNonQuery();
                    //MessageBox.Show($"Successfully Created Backup {SQLPath}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //outputText.Text += $"Step 4: Create DB backup completed...File output to {SQLPath}";
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                finally
                {
                    if (myConn.State == ConnectionState.Open)
                    {
                        myConn.Close();
                    }
                }
            }
        }
        private void DetachDB(string SlugName, string stagingProduction, string SQLServer) 
        {
            {
                String str;
                SqlConnection myConn = new SqlConnection($"Server={SQLServer};Integrated security=SSPI;database=master");

                str = $"EXEC sp_detach_db '{SlugName}-{stagingProduction}', 'true'";


                SqlCommand myCommand = new SqlCommand(str, myConn);
                myCommand.CommandTimeout = 600;
                try
                {
                    myConn.Open();
                    myCommand.ExecuteNonQuery();
                    //MessageBox.Show("Recovery set to simple", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //outputText.Text += "Step 2: Set recovery mode to simple completed" + Environment.NewLine;
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                finally
                {
                    if (myConn.State == ConnectionState.Open)
                    {
                        myConn.Close();
                    }
                }
            }
        }
        //method to generate sql query text template the user can paste into SQL management studio to manually run upsize process
        private void GenerateTemplate(string SlugName, string SQLPath, string stagingProduction, string dbSize)
        {
            Int16 error = ErrorCheck();
            if (error == 0)
            {
                return;
            }
            else
            {
                string SlugName1 = $"\"{SlugName}-{stagingProduction}\"";

                outputText.Text = $@"
                --Step 1
                --Create database
                CREATE DATABASE [{SlugName}-{stagingProduction}] 
                ON(NAME = N'{SlugName}-{stagingProduction}', FILENAME = N'{SQLPath}\{SlugName}-{stagingProduction}.mdf', SIZE = {dbSize}GB)
                LOG ON(NAME = N'{SlugName}-{stagingProduction}_Log', FILENAME = N'{SQLPath}\{SlugName}-{stagingProduction}_Log.ldf', SIZE = 500MB)
                GO
                
                --Step 2
                --Set recovery model to simple 
                ALTER DATABASE [{SlugName}-{stagingProduction}] SET RECOVERY SIMPLE 
                GO

                -- !!!! NOW RUN UPSIZE !!!!

                -- Step 3
                --Create a full SQL Server backup to disk
                BACKUP DATABASE {SlugName1}
                TO DISK = '{SQLPath}\{SlugName}-{stagingProduction}.bak'
                WITH STATS
                GO";
                Clipboard.SetText(outputText.Text);
                MessageBox.Show("Ensure EXACTData has been upgraded & Exact Server has been stopped before running upsize!\n \nResults copied to clipboard!", "Reminder", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void sqlButton_Click(object sender, EventArgs e)
        {
            var folderPath = new FolderBrowserDialog();
            DialogResult  = folderPath.ShowDialog();
            sqlpathBox.Text = folderPath.SelectedPath;
        }
        private void Generate_Click(object sender, EventArgs e)
        {
            Int16 error = ErrorCheck();
            if (error == 0)
            {
                return;
            }
            else
            {
                GenerateTemplate(slugBox.Text, sqlpathBox.Text, stagingBox.Text, dbSelect.Text);
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
        //if all fields are valid runs each method, waits for upsize process to finish before starting backup process
        private void AllInOne_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            string SlugBox = slugBox.Text;
            string SQLPath = sqlpathBox.Text;
            string staging = stagingBox.Text;
            string DBselect = dbSelect.Text;
            string SQlServer = SQLServer.Text;
            Int16 SiteID = Convert.ToInt16(siteID.Text);
            string newLine = Environment.NewLine;
            Stopwatch stopWatch = new Stopwatch();     
            Int16 error = ErrorCheck();
            if (error == 0)
            {
                return;
            }
            stopWatch.Start();
            outputText.Text = "CREATING DB PLEASE WAIT....";
            Task DBM = Task.Run(() =>
            {
                CreateDbm(SlugBox, SQLPath, staging, DBselect, SQlServer);
            });
            progressBar1.Increment(25);
            DBM.Wait();
            RestoreDbm(slugBox.Text, stagingBox.Text, SQLServer.Text);
            progressBar1.Increment(25);
            outputText.Text = "RUNNING UPSIZE PLEASE WAIT....";
            RunUpsize();
            while  (Process.GetProcessesByName("parallelupsizer").Length > 0) { }
            progressBar1.Increment(25);
            outputText.Text = "CREATING BACKUP PLEASE WAIT....";
            Task BDBM = Task.Run(() =>
            {
                BackupDBm(SlugBox, SQLPath, staging, SQlServer);
            });
            BDBM.Wait();
            DetachDB(SlugBox, staging, SQlServer);
            progressBar1.Increment(25);
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
            //MessageBox.Show($"Successfully Created Backup in {SQLPath}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            outputText.Text = $"Creating backup completed...Files output to {SQLPath}"+newLine+""+newLine+$"Finished in...{elapsedTime}";
            //ZipButton.Visible = true;
            Properties.Settings.Default.SQLPath = SQLServer.Text;
            Properties.Settings.Default.Save();

            DialogResult dialogResult = MessageBox.Show($"Successfully Created Backup in {SQLPath}, do you want to zip the backup?", "Zip backup?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {

                progressBar1.Value = 0;

                if (File.Exists($"{SQLPath}\\{SlugBox}-{staging}.bak"))
                {
                    outputText.Text += newLine + "Zipping backup please wait..."+newLine+"";
                    using (ZipArchive archive = ZipFile.Open($"{SQLPath}\\{SlugBox}-{staging}.zip", ZipArchiveMode.Create))
                    {
                        progressBar1.Increment(50);
                        Task Zip = Task.Run(() => { archive.CreateEntryFromFile($"{SQLPath}\\{SlugBox}-{staging}.bak", $"{SlugBox}-{staging}.bak", CompressionLevel.Optimal); });
                        Zip.Wait();
                        progressBar1.Increment(50);
                        MessageBox.Show($"File has been zipped in the following location {SQLPath}\\{SlugBox}-{staging}.zip", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        outputText.Text += newLine + $"File has been zipped in the following location {SQLPath}\\{SlugBox}-{staging}.zip";
                    }
                }
                else { MessageBox.Show("SQL Backup Doesn't Exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }

            }
            else if (dialogResult == DialogResult.No)
            {

            }

            DialogResult dialogResult1 = MessageBox.Show("Do you want to open Neymar to upload the data?", "Upload to Neymar?", MessageBoxButtons.YesNo);
            if (dialogResult1 == DialogResult.Yes)
            {
                if (SiteID == 0) { MessageBox.Show("Please enter valid site ID", "Site ID 0", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                else if (staging == "staging") {
                    System.Diagnostics.Process.Start($"https://transfer.dentally.co/?site%5Benvironment%5D={staging}&site%5Bexternal_practice_id%5D={SiteID}&site%5Bpractice_slug%5D={SlugBox}");
                    System.Diagnostics.Process.Start("explorer", SQLPath);
                }
                else if (staging == "production") {
                    System.Diagnostics.Process.Start($"https://transfer.dentally.co/?site%5Benvironment%5D={staging}&site%5Bexternal_practice_id%5D={SiteID}&site%5Bpractice_slug%5D={SlugBox}");
                    System.Diagnostics.Process.Start("explorer", SQLPath);
                }
                
            }
            else if (dialogResult1 == DialogResult.No)
            {
                
            }
        }
        private void ZipButton_Click(object sender, EventArgs e)
        {
            string SlugBox = slugBox.Text;
            string SQLPath = sqlpathBox.Text;
            string staging = stagingBox.Text;
            
            progressBar1.Value = 0;

            if (File.Exists($"{SQLPath}\\{SlugBox}-{staging}.bak"))
            {
                outputText.Text = "Zipping backup please wait...";
                using (ZipArchive archive = ZipFile.Open($"{SQLPath}\\{SlugBox}-{staging}.zip", ZipArchiveMode.Create))
                {
                    progressBar1.Increment(50);
                    Task Zip = Task.Run(() => { archive.CreateEntryFromFile($"{SQLPath}\\{SlugBox}-{staging}.bak", $"{SlugBox}-{staging}.bak", CompressionLevel.Fastest); });
                    Zip.Wait();
                    progressBar1.Increment(50);
                    MessageBox.Show($"File has been zipped in the following location {SQLPath}\\{SlugBox}-{staging}.zip", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    outputText.Text = $"File has been zipped in the following location {SQLPath}\\{SlugBox}-{staging}.zip";
                }
            }
            else { MessageBox.Show("SQL Backup Doesn't Exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            ServiceController serviceController = new ServiceController("MSSQLSERVER");

            DialogResult dialogResult = MessageBox.Show("Are you sure you want to restart the SQL service?", "Restart Service?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    if ((serviceController.Status.Equals(ServiceControllerStatus.Running)) || (serviceController.Status.Equals(ServiceControllerStatus.StartPending)))
                    {
                        serviceController.Stop();
                    }
                    serviceController.WaitForStatus(ServiceControllerStatus.Stopped);
                    serviceController.Start();
                    serviceController.WaitForStatus(ServiceControllerStatus.Running);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                return;
            }
        }
    }
    }


