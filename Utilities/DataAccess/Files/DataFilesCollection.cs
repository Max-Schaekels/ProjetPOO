using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetPOO.Utilities.DataAccess.Files
{
    public class DataFilesCollection : List<DataFile>
    {
        public DataFilesCollection()
        {
        }

        public void AddFile(DataFile df)
        {
            this.Add(df);
        }

        public string? GetFilePathByCodeFunction(string concern) => this.Find(df => df.Concern.Equals(concern))?.FullPath;

    }
}
