using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace T1
{

    delegate void StartFinish();

    delegate bool Filter(FileInfo item);


    class FileSystemVisitor
    {

        // private List<CatalogItem> findedItems = new List<CatalogItem>();

        //вложенность файла или папки
        private int deep = 0;

        public event StartFinish Start;
        public event StartFinish Finish;

        //экземпляр делегата для фильтрации
        private Filter MyFilter = null;


        public FileSystemVisitor()
        {

        }

        public FileSystemVisitor(Filter filter)
        {
            MyFilter = filter;
        }

        //public List<CatalogItem> GetFindedItems()
        //{
        //    return findedItems;
        //}       


        //public void FindItems(string derictoryPath)
        //{
        //    DirectoryInfo directory = new DirectoryInfo(derictoryPath);

        //    var directories = directory.GetDirectories();
        //    var files = directory.GetFiles();            

        //    deep++;          

        //    if (directories.Length == 0)
        //    {               
        //        foreach (var file in files)
        //        {
        //            findedItems.Add(new CatalogItem { Name = file.Name, PathFull = file.FullName, Deep = deep, Item = CatalogItems.File });
        //        }

        //        deep--;
        //        return;
        //    }
        //    else
        //    {
        //        foreach (var drctr in directories)
        //        {
        //            findedItems.Add(new CatalogItem { Name = drctr.Name, PathFull = drctr.FullName, Deep = deep, Item = CatalogItems.Directory });
        //            FindItems(drctr.FullName);
        //        }          

        //        foreach (var file in files)
        //        {
        //            findedItems.Add(new CatalogItem { Name = file.Name, PathFull = file.FullName, Deep = deep, Item = CatalogItems.File });
        //        }

        //        deep--;

        //    }




        //}




        //переберает файлы
        private IEnumerable<CatalogItem> FindFiles(FileInfo[] files)
        {
            foreach (var file in files)
            {

                //если алгоритм фильтрации задан
                if (MyFilter != null)
                {
                    if (MyFilter(file))
                    {                        
                        yield return new CatalogItem { Name = file.Name, PathFull = file.FullName, Deep = deep, Item = CatalogItems.File };                      
                        
                    }
                }
                else
                {
                    yield return new CatalogItem { Name = file.Name, PathFull = file.FullName, Deep = deep, Item = CatalogItems.File };
                }


            }
        }



        public IEnumerable<CatalogItem> FindItems(string derictoryPath)
        {
            //событие начала поиска
            if (deep == 0)
            {
                Start?.Invoke();
            }

            //Определяем начальную точку поиска
            DirectoryInfo directory = new DirectoryInfo(derictoryPath);

            //получаем имеющиеся в данной точке деректроии
            var directories = directory.GetDirectories();
            deep++;

            //если деректроия не пуста
            if (directories.Length == 0)
            {
                //получаем файлы в данной деректории
                var files = directory.GetFiles();

                foreach (var item in FindFiles(files))
                {
                    yield return item;
                }


                deep--;

            }
            //если деректория содержит элементы
            else
            {
                //проходим по всем деректориям 
                foreach (var drctr in directories)
                {
                    yield return new CatalogItem { Name = drctr.Name, PathFull = drctr.FullName, Deep = deep, Item = CatalogItems.Directory };

                    //проходим по элементам в деректории
                    foreach (var item in FindItems(drctr.FullName))
                    {
                        yield return item;
                    }
                }

                //получаем файлы в данной деректории
                var files = directory.GetFiles();

                foreach (var item in FindFiles(files))
                {
                    yield return item;
                }

                deep--;

                //завершение поиска
                if (deep == 0)
                {
                    Finish?.Invoke();
                }


            }

        }

    }
}
