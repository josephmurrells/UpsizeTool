using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SQLTemplateUI
{
    public class Zip
    {
        string SQLPath { get; set; }
        string SlugBox { get; set; }
        string staging { get; set; }

        public Zip (string sqlpath, string slugbox, string staging)
        {
            this.SQLPath = sqlpath;
            this.SlugBox = slugbox;
            this.staging = staging;
        }

        public void ZipBackup()
        {

             if (File.Exists($"{SQLPath}\\{SlugBox}-{staging}.bak"))
               {
                using (ZipArchive archive = ZipFile.Open($"{SQLPath}\\{SlugBox}-{staging}.zip", ZipArchiveMode.Create))
                  {
                    Task Zip = Task.Run(() => { archive.CreateEntryFromFile($"{SQLPath}\\{SlugBox}-{staging}.bak", $"{SlugBox}-{staging}.bak", CompressionLevel.Optimal); });
                    Zip.Wait();
                    MessageBox.Show($"File has been zipped in the following location {SQLPath}\\{SlugBox}-{staging}.zip", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                  }
                }
             else { MessageBox.Show("SQL Backup Doesn't Exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }

        }
    }
}
