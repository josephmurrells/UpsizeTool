using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using System.IO;

namespace SQLTemplateUI
{
    public class SQL
    {
        public string SlugName { get; set; }
        public string SQLPath { get; set; }
        public string ExactData { get; set; }
        public string SQLServer { get; set; }  
        public string stagingProduction { get; set; }
        public string dbSize { get; set; }
        public string Threads { get; set; }

        public SQL(string SlugName, string SQLPath, string SQLServer, string stagingProduction, string dbSize, string ExactData, string Threads)
        {
            this.SlugName = SlugName;
            this.SQLPath = SQLPath;
            this.ExactData = ExactData;
            this.SQLServer = SQLServer;
            this.stagingProduction = stagingProduction;
            this.dbSize = dbSize;
            this.Threads = Threads;
        }

        public void CreateDb()
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

        public void RestoreDb()
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

        public void BackupDb()
        {
            {
                String str;
                SqlConnection myConn = new SqlConnection($"Server={SQLServer};Integrated security=SSPI;database=master");

                str = $@"BACKUP DATABASE ""{SlugName}-{stagingProduction}""" +
                    $"TO DISK = '{SQLPath}\\{SlugName}-{stagingProduction}.bak'" +
                    "WITH STATS";

                SqlCommand myCommand = new SqlCommand(str, myConn);
                myCommand.CommandTimeout = 600;
                try
                {
                    myConn.Open();
                    myCommand.ExecuteNonQuery();
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

        public void DetachDb()
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

        public void RunUpsize()
        {
            {
                string upsizePath = Environment.CurrentDirectory;
                string json = File.ReadAllText($"{upsizePath}\\ParallelUpsizer\\upsizerinput.json");
                dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                jsonObj["SourceExactDataPath"] = ExactData ;
                jsonObj["DestinationSqlUpsizedDbName"] = $"{SlugName}-{stagingProduction}";
                jsonObj["DestinationSqlServerInstance"] = SQLServer;
                jsonObj["Parallel"] = Convert.ToInt32(Threads);
                string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText($"{upsizePath}\\ParallelUpsizer\\upsizerinput.json", output);


                System.Diagnostics.Process.Start($"{upsizePath}\\ParallelUpsizer\\parallelupsizer.exe", $"{upsizePath}\\ParallelUpsizer\\upsizerInput.json");
            }
        }
    }
}
