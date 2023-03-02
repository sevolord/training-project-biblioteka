using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteka
{
    internal interface User //интерфейс, от которого будут унаследованы читатель и библиотекарь
    {
        public void SaveInFile();
        public void DeleteFileUser();
        public string GetFIO();
    }
}
