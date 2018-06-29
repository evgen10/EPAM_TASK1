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

            DirectoryInfo directory = new DirectoryInfo(@"D:\Tasks\01. Advanced C#");

            FileSystemVisitor fileInfo = new FileSystemVisitor();


            fileInfo.FindItems(@"D:\Tasks");

            foreach (var item in fileInfo.GetFindedItems())
            {
                Console.WriteLine(item.Deep + " " + item.Name);
            }






            Console.Read();
        }
    }
}
