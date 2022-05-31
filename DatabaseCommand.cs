using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace ClearFiles
{
    class DatabaseCommand
    {
        public virtual List<FileInfo> FilesForDelete()
        {
            return new List<FileInfo>();
        }

        public virtual string DelFileOnDB(int Id)
        {
            return "";
        }
    }
}
