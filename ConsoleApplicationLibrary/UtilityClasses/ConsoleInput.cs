using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplicationLibrary.UtilityClasses
{

    /// <summary>
    /// Предоставляет методы для корректного ввода данных
    /// </summary>
    public class ConsoleInput
    {
        /// <summary>
        /// Метод обеспечивает верный ввод целого положительного числа
        /// </summary>
        /// <returns>Целое положительное число</returns>
        public static int EnterPositiveIntengerNumber()
        {
            //цикл выполняется пока не будет введено целое положительное число
            while (true)
            {
                int number;
                Console.Write("Enter a number: ");
                if (int.TryParse(Console.ReadLine(), out number))
                {
                    if (number >= 0)
                    {
                        return number;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Incorrect value: ");
                        Console.ResetColor();
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Incorrect value");
                    Console.ResetColor();
                }
            }
        }


        /// <summary>
        /// Метод обеспечивает верный ввод целого числа
        /// </summary>
        /// <returns>Целое число</returns>
        public static int EnterIntengerNumber()
        {
            while (true)
            {
                int number;
                Console.Write("Введите целое число: ");
                if (int.TryParse(Console.ReadLine(), out number))
                {
                    return number;

                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Неверные данные");
                    Console.ResetColor();
                }
            }
        }



        /// <summary>
        /// Метод обеспечивает верный ввод вещественного положительного числа
        /// </summary>
        /// <returns>Вещественное положительное число</returns>
        public static double EnterPositiveRealNumber()
        {
            while (true)
            {
                double number;
                Console.Write("Введите положительное число: ");
                if (double.TryParse(Console.ReadLine(), out number))
                {
                    if (number >= 0)
                    {
                        return number;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Вы ввели отрицательное число: ");
                        Console.ResetColor();
                    }

                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Неверные данные");
                    Console.ResetColor();
                }
            }
        }




        /// <summary>
        /// Метод обеспечивает верный ввод вещественного числа
        /// </summary>
        /// <returns>Вещественное число</returns>
        public static double EnterRealNumber()
        {
            while (true)
            {
                double number;
                Console.Write("Введите число: ");
                if (double.TryParse(Console.ReadLine(), out number))
                {                 
                    
                        return number;                                 

                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Неверные данные");
                    Console.ResetColor();
                }
            }
        }
    }
}
