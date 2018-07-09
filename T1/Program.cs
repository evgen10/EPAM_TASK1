using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;


using ConsoleApplicationLibrary.UtilityClasses;

namespace T1
{
    public class Program
    {
        //флаг для остановки поиска
        static bool searchStopper = false;
        //флаг для исключения элемента из списка
        static bool addStopper = true;

        //хранит расширение для фильтра
        static string extensionForFilter;
        //хранит расширение для остановки или исключения файла
        static string extensionForFile;


        //хранит начальную директорию
        
        //static string path = @"D:\Tasks\Code\Task1\Epam_Task1\Task1_Tests\bin\Debug\TestFolder";     
        static string startDirectory = @"C:\Users\iammr\Desktop\Новая папка\Epam_Task1\Task1_Tests\bin\Debug\TestFolder";

        static void Main(string[] args)
        {    
           // string path = @"C:\Users\Evgeniy_Chernyshkov\Desktop\New Folder (3)";
            FileSystemVisitor fileSystemVisitor = new FileSystemVisitor();

            do
            {
                fileSystemVisitor.FileFinded -= StopSearchForDocument;
                fileSystemVisitor.FileFinded -= ExcludeFile;
                fileSystemVisitor.DirectoryFinded -= ExcludeDirecotry;
                fileSystemVisitor.DirectoryFinded -= StopSearchForDirectory;

                try
                {
                    Console.WriteLine(@"Enter 1  to search WITHOUT filtering
Enter 2  to search WITH filtering");


                    switch (ConsoleInput.EnterPositiveIntengerNumber())
                    {
                        case 1:
                            {

                                fileSystemVisitor = new FileSystemVisitor();
                               

                                Console.WriteLine(@"Select action for event. 
1-Stop search if file extension equals entered extension.
2-Exclude files if file extension equals entered extension.
3-Stop search if directory is empty.
4-Exclude directory if it is empty
5-Show all files and directories

");
                                switch (ConsoleInput.EnterPositiveIntengerNumber())
                                {

                                    case 1:
                                        {
                                            Console.WriteLine(@"Enter extension for file. For example .txt");
                                            extensionForFile = Console.ReadLine();
                                            fileSystemVisitor.FileFinded += StopSearchForDocument;
                                            break;
                                        }
                                    case 2:
                                        {
                                            Console.WriteLine(@"Enter extension for file. For example .txt");
                                            extensionForFile = Console.ReadLine();
                                            fileSystemVisitor.FileFinded += ExcludeFile;
                                            break;
                                        }
                                    case 3:
                                        {
                                            fileSystemVisitor.DirectoryFinded += StopSearchForDirectory;
                                            break;
                                        }
                                    case 4:
                                        {
                                            fileSystemVisitor.DirectoryFinded += ExcludeDirecotry;
                                            break;
                                        }
                                    case 5:
                                        {
                                            break;
                                        }

                                    default:
                                        break;
                                }


                                break;

                            }

                        case 2:
                            {

                                Console.WriteLine(@"Enter extension to filter. For example .txt");
                                extensionForFilter = Console.ReadLine();

                                SearchFilter filter = new SearchFilter(extensionForFilter);

                                fileSystemVisitor = new FileSystemVisitor(filter.MFilter);
                                break;
                            }

                        default:
                            {                               
                                continue;                             
                            }

                    }
         
                    //присвоение обработчиков событиям
                    fileSystemVisitor.Start += OnStarted;
                    fileSystemVisitor.Finish += OnFinished;

                    fileSystemVisitor.DirectoryFinded += ChangeDirectoryColor;

                
                    fileSystemVisitor.FilteredFileFinded += ChangeColorForFiltredFile;

                    fileSystemVisitor.FilteredDirectoryFinded += ChangeColorForFiltredDirectory;
            

                    //Вызваем итератор
                    foreach (var item in fileSystemVisitor.FindItems(startDirectory))
                    {
                        
                        if (addStopper)
                        {
                            //количество отступов в зависимости от уровня вложенности
                            StringBuilder space = new StringBuilder();
                            space.Append(' ', item.NestingLevel);

                            Console.WriteLine(space + item.Name);

                        }

                        Console.ResetColor();

                        
                        if (searchStopper)
                        {
                            searchStopper = false;
                            OnFinished();
                            break;
                        }

                        
                        addStopper = true;
                        

                    }
                    
                    Console.Read();
                }
                catch (Exception)
                {
                    Console.WriteLine("Directory not found.");
                    Console.Read();
                }

            } while (ProgramExit.Exit());



        }        




        //обработчик события найденого файла
        //останавливает поиск
        private static void StopSearchForDocument(object sender, FileEventArgs e)
        {
            if (e.Extention == extensionForFile)
            {
                searchStopper = true;
            }
        }


        //обработчик события найденого файла
        //исключает файл из итогового списка
        private static void ExcludeFile(object sender, FileEventArgs e)
        {
            if (e.Extention == extensionForFile)
            {
                addStopper = false;
            }


        }


        //обработчик события найденой директории
        //исключает директорию из итоговго списка
        private static void ExcludeDirecotry(object sender, DirectoryEventArgs e)
        {
            if (e.IsEmpty)
            {
                addStopper = false;
            }
        }



        //обработчик события найденой директории
        //меняет цвет отображения
        private static void ChangeDirectoryColor(object sender, DirectoryEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
        }


        //обработчик события найденой директории
        //останавливает поиск
        private static void StopSearchForDirectory(object sender, DirectoryEventArgs e)
        {
            if (e.IsEmpty)
            {
                searchStopper = true;
            }

            Console.ForegroundColor = ConsoleColor.DarkYellow;
        }

        //обработчик события найденого файла после фильтрации
        //останавливает поиск
        private static void FilteredFileStopSearch(object sender, FileEventArgs e)
        {
            if (e.Extention == extensionForFilter)
            {
                searchStopper = true;
            }

        }

        //обработчик события найденого файла после фильтрации
        //меняет цвет отображения файла
        private static void ChangeColorForFiltredFile(object sender, FileEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
        }

        //обработчик события найденой директории после фильтрации
        //меняет цвет отображения директории
        private static void ChangeColorForFiltredDirectory(object sender, DirectoryEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.White;
        }

        //обработчик события завершения поиска
        private static void OnFinished()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("The search finished!\n");
            Console.ResetColor();

        }


        //обработчик события старта поиска
        private static void OnStarted()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nThe search started!");
            Console.ResetColor();
        }

    }
}
