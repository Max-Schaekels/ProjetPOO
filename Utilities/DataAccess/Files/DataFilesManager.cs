using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetPOO.Utilities.DataAccess.Files
{
    public class DataFilesManager
    {
        public DataFilesManager(string configFile)
        {
            List<string> listToRead = new List<string>();

            listToRead = System.IO.File.ReadAllLines(configFile).ToList();

            //directory path is in the first line of the config file 
            string directory = listToRead[0].Split(',')[1];
            DataFile.FilesPathDir = directory;

            listToRead.RemoveAt(0);
            foreach (string s in listToRead)
            {
                string[] fields = s.Split(',');

                DataFiles.AddFile(new DataFile(fileName: fields[1], concern: fields[0]));
            }

        }

        public DataFilesCollection DataFiles { get; set; } = new DataFilesCollection();
    }
}
