using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace T1
{
    class Program
    {       
       

        static void Main(string[] args)
        {
            //try
            //{

            //    FileSystemVisitor fileInfo = new FileSystemVisitor();               

            //    fileInfo.FindItems(@"D:\Tasks\01. Advanced C#");

            //    foreach (var item in fileInfo.GetFindedItems())
            //    {
            //        StringBuilder space = new StringBuilder();
            //        space.Append(' ', item.Deep);              


            //        Console.WriteLine(space+item.Name);
            //    }

            //    Console.Read();
            //}
            //catch (Exception)
            //{

            //    Console.WriteLine("Directory not found.");
            //    Console.Read();
            //}

            string path = @"D:\Tasks\01. Advanced C#";
            

            try
            {

                FileSystemVisitor fileInfo = new FileSystemVisitor();
                //FileSystemVisitor fileInfo = new FileSystemVisitor(MFilter);

                fileInfo.Start += OnStarted;
                fileInfo.Finish += OnFinished;             


                foreach (var item in fileInfo.FindItems(path))              
                {
                    StringBuilder space = new StringBuilder();
                    space.Append(' ', item.Deep);


                    Console.WriteLine(space + item.Name);
                }
     

                Console.Read();
            }
            catch (Exception)
            {

                Console.WriteLine("Directory not found.");
                Console.Read();
            }       
            

        }



        private static bool MFilter(FileInfo file)
        {
            if (file.Extension==".pptx")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        

        private static void OnFileFinded()
        {

            if (false)
            {

            }
            
               
        }


        private static void OnFinished()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("The search finished!\n");
            Console.ResetColor();
        }

        private static  void OnStarted()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nThe search started!");
            Console.ResetColor();
        }
    }
}
