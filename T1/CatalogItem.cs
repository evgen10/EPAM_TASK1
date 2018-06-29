using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T1
{
    enum CatalogItems
    {
        Directory,
        File
    }


    class CatalogItem
    {
        public string Name { get; set; }

        public string PathFull { get; set; }

        public CatalogItems  Item { get; set; }

        public int Deep { get; set; }

    }
}
