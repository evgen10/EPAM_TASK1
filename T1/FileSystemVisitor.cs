using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace T1
{
    class FileSystemVisitor
    {

        private List<CatalogItem> findedItems = new List<CatalogItem>();
        private int deep = 0;


        public List<CatalogItem> GetFindedItems()
        {
            return findedItems;
        }       
      

        public void FindItems(string derictoryPath)
        {
            DirectoryInfo directory = new DirectoryInfo(derictoryPath);

            var directories = directory.GetDirectories();
            var files = directory.GetFiles();            

            deep++;          

            if (directories.Length == 0)
            {               
                foreach (var file in files)
                {
                    findedItems.Add(new CatalogItem { Name = file.Name, PathFull = file.FullName, Deep = deep, Item = CatalogItems.File });
                }

                deep--;
                return;
            }
            else
            {
                foreach (var drctr in directories)
                {
                    findedItems.Add(new CatalogItem { Name = drctr.Name, PathFull = drctr.FullName, Deep = deep, Item = CatalogItems.Directory });
                    FindItems(drctr.FullName);
                }          

                foreach (var file in files)
                {
                    findedItems.Add(new CatalogItem { Name = file.Name, PathFull = file.FullName, Deep = deep, Item = CatalogItems.File });
                }

                deep--;

            }

            


            //}

            //public IEnumerable<CatalogItem> FindItems(string derictoryPath)
            //{
            //    DirectoryInfo directory = new DirectoryInfo(derictoryPath);


            //    var directories = directory.GetDirectories();
            //    deep++;

            //    if (directories.Length == 0)
            //    {
            //        var files = directory.GetFiles();

            //        foreach (var file in files)
            //        {

            //            yield return new CatalogItem { Name = file.Name, PathFull = file.FullName, Deep = deep, Item = CatalogItems.File };
            //        }


            //        deep--;

            //    }
            //    else
            //    {

            //        foreach (var drctr in directories)
            //        {
            //            yield return new CatalogItem { Name = drctr.Name, PathFull = drctr.FullName, Deep = deep, Item = CatalogItems.Directory };
            //            FindItems(drctr.FullName);
            //        }

            //        var files = directory.GetFiles();

            //        foreach (var file in files)
            //        {
            //            yield return new CatalogItem { Name = file.Name, PathFull = file.FullName, Deep = deep, Item = CatalogItems.File };
            //        }

            //        deep--;

            //    }





            }




        }
    }
