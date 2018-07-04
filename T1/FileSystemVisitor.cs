using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace T1
{

    public delegate void StartFinish();
    public delegate bool Filter(FileInfo item);
    public delegate void FindedItem();

    public class FileEventArgs : EventArgs
    {
        public string extention;
    }

    public class DirectoryEventArgs : EventArgs
    {
        public bool isEmpty;
    }


    public class FileSystemVisitor
    {
        //вложенность файла или папки
        private int deep = 0;
        //указывает используется ли фильтр
        private bool filtred;


        private FileEventArgs fileArgs = new FileEventArgs();
        private DirectoryEventArgs directoryArgs = new DirectoryEventArgs();

        
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
                        fileArgs.extention = file.Extension;
                        FilteredFileFinded?.Invoke(this, fileArgs);
                        yield return new CatalogItem { Name = file.Name, Deep = deep, Item = CatalogItems.File };

                    }
                }
                else
                {
                    fileArgs.extention = file.Extension;
                    FileFinded?.Invoke(this, fileArgs);
                    yield return new CatalogItem { Name = file.Name, Deep = deep, Item = CatalogItems.File };
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
                    FilteredDirectoryFinded?.Invoke(this, directoryArgs);
                }
                else
                {
                    DirectoryFinded?.Invoke(this, directoryArgs);
                }

                yield return new CatalogItem { Name = directory.Name, Deep = deep, Item = CatalogItems.Directory };
            }


            if (directory.GetFiles().Length == 0 && directory.GetDirectories().Length == 0)
            {
                directoryArgs.isEmpty = true;
                if (filtred)
                {
                    FilteredDirectoryFinded?.Invoke(this, directoryArgs);
                }
                else
                {
                    DirectoryFinded?.Invoke(this, directoryArgs);
                }           
                yield return new CatalogItem { Name = directory.Name, Deep = deep, Item = CatalogItems.Directory };
            }
            else
            {
                directoryArgs.isEmpty = false;
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
                            directoryArgs.isEmpty = true;
                        }

                        
                        if (filtred)
                        {
                            FilteredDirectoryFinded?.Invoke(this, directoryArgs);
                        }
                        else
                        {
                            DirectoryFinded?.Invoke(this, directoryArgs);
                        }
                        yield return new CatalogItem { Name = drctr.Name, Deep = deep, Item = CatalogItems.Directory };

                        if (!dirEmpty)                      
                        {
                            directoryArgs.isEmpty = false;
                            //проходим по элементам в директории
                            foreach (var item in FindItems(drctr.FullName))
                            {
                                yield return item;
                            }
                        }

                        directoryArgs.isEmpty = false;

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
