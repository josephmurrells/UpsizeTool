using System;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;


namespace SQLTemplateUI
{
    public class ErrorChecking
    {
        public string SlugName { get; set; }
        public string SQLPath { get; set; }
        public string ExactData { get; set; }
        public string SQLServer { get; set; }
        public string stagingProduction { get; set; }
        public string dbSize { get; set; }
        public ErrorChecking(string SlugName, string SQLPath,string ExactData, string SQLServer, string stagingProduction, string dbSize)
        {
            this.SlugName = SlugName;
            this.SQLPath = SQLPath;
            this.ExactData = ExactData;
            this.SQLServer = SQLServer;
            this.stagingProduction = stagingProduction;
            this.dbSize = dbSize;
        }
        public Int16 ErrorCheck()
        {
            Process[] processes = Process.GetProcessesByName("SOEDBService");
            if (SlugName == "")
            {
                MessageBox.Show("Slug name is blank", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
            if (!Directory.Exists(SQLPath))
            {
                MessageBox.Show("SQL path doesn't exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
            if (!Directory.Exists(ExactData))
            {
                MessageBox.Show("EXACTData path doesn't exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
            if (stagingProduction == "")
            {
                MessageBox.Show("Please select environment", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
            if (dbSize == "0")
            {
                MessageBox.Show("Please enter valid DB size", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
            if (processes.Length > 0)
            {
                MessageBox.Show("EXACT DB still running, stop before continuing!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return 0;
            }
            if (SQLServer == "")
            {
                MessageBox.Show("Please enter SQL server!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return 0;
            }
            else
            {
                return 1;
            }
        }
    }
}
