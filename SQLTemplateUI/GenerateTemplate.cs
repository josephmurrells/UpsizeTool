using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLTemplateUI
{
    public class GenerateTemplate
    {
        public string SlugName { get; set; } 
        public string SQLPath { get; set; } 
        string stagingProduction { get; set; }  
        string dbSize { get; set; }
        public GenerateTemplate(string slugname, string sqlpath, string stagingproduction, string dbsize)
        {
            this.SlugName = slugname;
            this.SQLPath = sqlpath;
            this.stagingProduction = stagingproduction;
            this.dbSize = dbsize;
        }

        public string TemplateGenerator()
        {
            string template = $@"
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
                BACKUP DATABASE ""{SlugName}-{stagingProduction}""
                TO DISK = '{SQLPath}\{SlugName}-{stagingProduction}.bak'
                WITH STATS
                GO";
            return template;
        }
    }
}
