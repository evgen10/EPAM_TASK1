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
            try
            {
                
                FileSystemVisitor fileInfo = new FileSystemVisitor();               

                fileInfo.FindItems(@"D:\Music");

                foreach (var item in fileInfo.GetFindedItems())
                {
                    StringBuilder space = new StringBuilder();
                    space.Append(' ', item.Deep);              


                    Console.WriteLine(space+item.Name);
                }
               
                Console.Read();
            }
            catch (Exception)
            {

                Console.WriteLine("Directory not found.");
                Console.Read();
            }
       
        }
    }
}
