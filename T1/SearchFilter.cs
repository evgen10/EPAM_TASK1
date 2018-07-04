using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace T1
{
    public class SearchFilter
    {
        private string extension;


        public SearchFilter(string extension)
        {
            this.extension = extension;
        }

        public bool MFilter(FileInfo file)
        {
            if (file.Extension == extension)
            {
                return true;
            }
            else
            {
                return false;
            }
        }




    }
}
