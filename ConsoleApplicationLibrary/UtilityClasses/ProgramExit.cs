using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplicationLibrary.UtilityClasses
{

    /// <summary>
    /// Содержит методы для выхода из программы.
    /// </summary
    public class ProgramExit
    {
        /// <summary>
        /// Метод возвращает false если нажата любая клавиша кроме "ESC"
        /// </summary>
        /// <returns></returns>
        public static bool Exit()
        {
            Console.WriteLine("Press any key to continue. Press ESC to exit.\n");
            if (Console.ReadKey(true).Key == ConsoleKey.Escape)
            {
                return false;
            }

            return true;

        }
    }
}
