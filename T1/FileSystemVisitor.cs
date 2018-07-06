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
        public string Extention { get; set; }
    }

    public class DirectoryEventArgs : EventArgs
    {
        public bool IsEmpty { get; set; }
    }


    public class FileSystemVisitor
    {
        //вложенность файла или папки
        private int nestingLevel = 0;
        //указывает используется ли фильтр
        private bool filtered;


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
            filtered = true;
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
                        fileArgs.Extention = file.Extension;
                        FilteredFileFinded?.Invoke(this, fileArgs);
                        yield return new CatalogItem { Name = file.Name, NestingLevel = nestingLevel, Item = CatalogItems.File };

                    }
                }
                else
                {
                    fileArgs.Extention = file.Extension;
                    FileFinded?.Invoke(this, fileArgs);
                    yield return new CatalogItem { Name = file.Name, NestingLevel = nestingLevel, Item = CatalogItems.File };
                }

            }
        }


        public IEnumerable<CatalogItem> FindItems(string directoryPath)
        {

            //Определяем начальную точку поиска
            DirectoryInfo startDirectory = new DirectoryInfo(directoryPath);

            if (nestingLevel == 0)
            {
                //событие начала поиска
                Start?.Invoke();

                CallEventDirectoryFound();

                yield return new CatalogItem { Name = startDirectory.Name, NestingLevel = nestingLevel, Item = CatalogItems.Directory };
            }

            //если директория пустая
            if (startDirectory.GetFiles().Length == 0 && startDirectory.GetDirectories().Length == 0)
            {
                directoryArgs.IsEmpty = true;

                CallEventDirectoryFound();

                yield return new CatalogItem { Name = startDirectory.Name, NestingLevel = nestingLevel, Item = CatalogItems.Directory };
            }
            else
            {
                directoryArgs.IsEmpty = false;
                //получаем имеющиеся в данной точке директроии
                var directories = startDirectory.GetDirectories();

                nestingLevel++;


                //если директория не содержит других директорий
                if (directories.Length == 0)
                {
                    //получаем файлы в данной директории
                    var files = startDirectory.GetFiles();

                    foreach (var item in FindFiles(files))
                    {
                        yield return item;
                    }

                    nestingLevel--;

                }
                //если директория содержит элементы
                else
                {
                    //проходим по всем директориям 
                    foreach (var directory in directories)
                    {
                        bool directoryEmpty = directory.GetFiles().Length == 0 && directory.GetDirectories().Length == 0;


                        if (directoryEmpty)
                        {
                            directoryArgs.IsEmpty = true;
                        }

                        CallEventDirectoryFound();

                        yield return new CatalogItem { Name = directory.Name, NestingLevel = nestingLevel, Item = CatalogItems.Directory };

                        if (!directoryEmpty)
                        {
                            directoryArgs.IsEmpty = false;
                            //проходим по элементам в директории
                            foreach (var item in FindItems(directory.FullName))
                            {
                                yield return item;
                            }
                        }

                        directoryArgs.IsEmpty = false;

                    }

                    //получаем файлы в данной директории
                    var files = startDirectory.GetFiles();

                    foreach (var item in FindFiles(files))
                    {
                        yield return item;
                    }

                    nestingLevel--;

                    //завершение поиска
                    if (nestingLevel == 0)
                    {
                        Finish?.Invoke();
                    }
                }

            }
        }

        private void CallEventDirectoryFound()
        {
            if (filtered)
            {
                FilteredDirectoryFinded?.Invoke(this, directoryArgs);
            }
            else
            {
                DirectoryFinded?.Invoke(this, directoryArgs);
            }
        }
    }
}
