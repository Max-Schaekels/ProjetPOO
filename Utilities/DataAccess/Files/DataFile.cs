using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetPOO.Utilities.DataAccess.Files
{
    public class DataFile
    {
        private string _fileName;
        private string _concern;
        public DataFile(string fileName, string concern)
        {
            FileName = fileName;
            Concern = concern;

        }

        public string FileName { get => _fileName; set => _fileName = value; }


        public string Concern { get => _concern; set => _concern = value; }

 
        public string FullPath => $"{FilesPathDir}{FileName}";


        public static string FilesPathDir { get; set; }
    }
}
