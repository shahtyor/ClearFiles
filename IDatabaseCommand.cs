using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearFiles
{
    interface IDatabaseCommand
    {
        List<FileInfo> FilesForDelete();

        string DelFileOnDB(int Id);
    }
}
