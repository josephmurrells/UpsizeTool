using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SQLTemplateUI
{
    public static class Neymar
    {
        public static void OpenNeymar(string staging, string SlugBox, string SiteID, string SQLPath)
        {
            DialogResult dialogResult1 = MessageBox.Show("Do you want to open Neymar to upload the data?", "Upload to Neymar?", MessageBoxButtons.YesNo);
            if (dialogResult1 == DialogResult.Yes)
            {
                if (SiteID == "0") { MessageBox.Show("Please enter valid site ID", "Site ID 0", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                else if (staging == "staging")
                {
                    System.Diagnostics.Process.Start($"https://transfer.dentally.co/?site%5Benvironment%5D={staging}&site%5Bexternal_practice_id%5D={SiteID}&site%5Bpractice_slug%5D={SlugBox}");
                    System.Diagnostics.Process.Start("explorer", SQLPath);
                }
                else if (staging == "production")
                {
                    System.Diagnostics.Process.Start($"https://transfer.dentally.co/?site%5Benvironment%5D={staging}&site%5Bexternal_practice_id%5D={SiteID}&site%5Bpractice_slug%5D={SlugBox}");
                    System.Diagnostics.Process.Start("explorer", SQLPath);
                }

            }
            else if (dialogResult1 == DialogResult.No)
            {

            }
        }
    }
}
