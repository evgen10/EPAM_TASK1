using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplicationLibrary.UtilityClasses
{
    /// <summary>
    /// Предоставляет методы для вывода в консоль
    /// </summary>
    public static class ConsoleOutput
    {
        /// <summary>
        /// Метод выводит в консоль одномерный массив
        /// </summary>
        /// <param name="array">Массив, который необходимо вывести</param>
        public static void OutputArray(int[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                Console.Write(array[i] + "  ");

            }

            Console.WriteLine();
        }

    }
}
