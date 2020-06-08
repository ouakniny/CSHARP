using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace SmartZip
{
    class Program
    {
        static void Main(string[] args)
        {
            string archivepath = @"E:\data\LogAnalysis\ARCHIVE\";
            string[] mylist = Directory.GetFiles(archivepath,"*.log");
            var months = new HashSet<string>();
            for (int i = 0; i < mylist.Length; i++)
            {
                string current_month = mylist[i].ToString().Substring(mylist[i].Length - 23, 7);
                /* there are 2 files formats ! RSPowerBI_YYYY_MM_DD_hh_mm_ss.log and RSPowerBIYYYY_MM_DD_hh_mm_ss.0.log
                   the second format only when there are several files for the same day
                 */
                int count_underscore = current_month.Split('_').Length - 1;
                if (count_underscore == 1) {
                    months.Add(current_month);
                }
            }
            /* Create one zip file for each month */
            foreach (string m in months)
            {
                var zipFile = archivepath+@"\RSPowerBI_"+m+".zip";
                var files = Directory.GetFiles(archivepath, "*"+m+"*.log");
                using (var archive = ZipFile.Open(zipFile, ZipArchiveMode.Update))
                {                                   
                    foreach (var fPath in files)
                    {                                                       
                        if(archive.GetEntry(Path.GetFileName(fPath)) == null)
                        {
                            archive.CreateEntryFromFile(fPath, Path.GetFileName(fPath));
                        }
                    }
                }
                /* delete files from the specific month */
                var dir = new DirectoryInfo(archivepath);
                foreach (var file in dir.EnumerateFiles("*" + m + "*.log"))
                {
                    file.Delete();
                }
            }










        }
    }
}
