using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T1
{
    public enum  CatalogItems
    {
        Directory,
        File
    }


    public class CatalogItem
    {
        public string Name { get; set; }       

        public CatalogItems  Item { get; set; }

        public int Deep { get; set; }


        public override int GetHashCode()
        {
            return base.GetHashCode();  
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            CatalogItem item = obj as CatalogItem;

            if (item as CatalogItem == null)
            {
                return false;
            }

            return Name == item.Name && Item == item.Item && Deep == item.Deep;

        }

    }


    
}
