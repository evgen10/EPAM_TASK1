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
    delegate void FindedItem();

    class FileEventArgs : EventArgs
    {
        public string extention;
    }

    class DirectoryEventArgs : EventArgs
    {
        public bool isEmpty;
    }


    class FileSystemVisitor
    {
        //вложенность файла или папки
        private int deep = 0;
        //указывает используется ли фильтр
        private bool filtred;


        private FileEventArgs args = new FileEventArgs();
        private DirectoryEventArgs dirArgs = new DirectoryEventArgs();



        public event StartFinish Start;
        public event StartFinish Finish;
        public event EventHandler<FileEventArgs> FileFinded;
        public event EventHandler<DirectoryEventArgs> DirectoryFinded;
        public event EventHandler<FileEventArgs> FilteredFileFinded;
        public event EventHandler<DirectoryEventArgs> FilteredDirectoryFinded;

        //экземпляр делегата для фильтрации
        private Filter MyFilter = null;


        public FileSystemVisitor()
        {

        }

        public FileSystemVisitor(Filter filter)
        {
            MyFilter = filter;
            filtred = true;
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
                        args.extention = file.Extension;
                        FilteredFileFinded?.Invoke(this, args);
                        yield return new CatalogItem { Name = file.Name, PathFull = file.FullName, Deep = deep, Item = CatalogItems.File };

                    }
                }
                else
                {
                    args.extention = file.Extension;
                    FileFinded?.Invoke(this, args);
                    yield return new CatalogItem { Name = file.Name, PathFull = file.FullName, Deep = deep, Item = CatalogItems.File };
                }

            }
        }


        public IEnumerable<CatalogItem> FindItems(string derictoryPath)
        {

            //Определяем начальную точку поиска
            DirectoryInfo directory = new DirectoryInfo(derictoryPath);

            if (deep == 0)
            {
                //событие начала поиска
                Start?.Invoke();
               

                if (filtred)
                {
                    FilteredDirectoryFinded?.Invoke(this, dirArgs);
                }
                else
                {
                    DirectoryFinded?.Invoke(this, dirArgs);
                }

                yield return new CatalogItem { Name = directory.Name, Deep = deep, Item = CatalogItems.Directory };
            }


            if (directory.GetFiles().Length == 0 && directory.GetDirectories().Length == 0)
            {
                dirArgs.isEmpty = true;
                if (filtred)
                {
                    FilteredDirectoryFinded?.Invoke(this, dirArgs);
                }
                else
                {
                    DirectoryFinded?.Invoke(this, dirArgs);
                }           
                yield return new CatalogItem { Name = directory.Name, Deep = deep, Item = CatalogItems.Directory };
            }
            else
            {
                dirArgs.isEmpty = false;
                //получаем имеющиеся в данной точке директроии
                var directories = directory.GetDirectories();

                deep++;

                //если директория не содержит других директорий
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
                        bool dirEmpty = drctr.GetFiles().Length == 0 && drctr.GetDirectories().Length == 0;

                        
                        if (dirEmpty)
                        {
                            dirArgs.isEmpty = true;
                        }

                        
                        if (filtred)
                        {
                            FilteredDirectoryFinded?.Invoke(this, dirArgs);
                        }
                        else
                        {
                            DirectoryFinded?.Invoke(this, dirArgs);
                        }
                        yield return new CatalogItem { Name = drctr.Name, Deep = deep, PathFull = drctr.FullName, Item = CatalogItems.Directory };

                        if (!dirEmpty)                      
                        {
                            dirArgs.isEmpty = false;
                            //проходим по элементам в директории
                            foreach (var item in FindItems(drctr.FullName))
                            {
                                yield return item;
                            }
                        }

                        dirArgs.isEmpty = false;

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
}
