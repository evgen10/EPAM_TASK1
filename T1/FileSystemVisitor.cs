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

        //перебирает файлы
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

            //получаем имеющиеся в данной точке директроии
            var directories = directory.GetDirectories();
            deep++;

            //если директория не пуста
            if (directories.Length == 0)
            {
                //получаем файлы в данной директории
                var files = directory.GetFiles();

                foreach (var item in FindFiles(files))
                {
                    yield return item;
                }

                deep--;

            }
            //если директория содержит элементы
            else
            {
                //проходим по всем директориям 
                foreach (var drctr in directories)
                {                

                    yield return new CatalogItem { Name = drctr.Name, PathFull = drctr.FullName, Deep = deep, Item = CatalogItems.Directory };

                    //проходим по элементам в директории
                    foreach (var item in FindItems(drctr.FullName))
                    {
                        ////если файл найден
                        //if (finded)
                        //{
                        //    Finish();
                        //    yield break;
                        //}

                        yield return item;
                    }
                }

                //получаем файлы в данной директории
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
