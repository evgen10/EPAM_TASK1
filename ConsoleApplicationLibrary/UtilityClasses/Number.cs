using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplicationLibrary.UtilityClasses
{

    /// <summary>
    /// Класс для работы с числами
    /// </summary>
    public class Number
    {
        /// <summary>
        /// Метод проверяет, является ли число простым
        /// </summary>
        /// <param name="number">Число, которое требует проверки/param>
        /// <returns>Возвращает true если число простое</returns>
        public static bool IsPrime(int number)
        {
            // простыми могут быть только положительные числа
            if (IsPositive(number))
            {
                if (number == 1)//единица не является простым числом
                {
                    return false;
                }
                //Делим входное число на числа из интервала от 2 до входного числа.
                //Если входное число без остатка делится на любое число кроме самого себя, возвращаем false
                for (int i = 2; i < number; i++)
                {
                    if (number % i == 0)
                        return false;
                }

                return true;
            }
            return false;
        }

        /// <summary>
        /// Метод проверяет, является ли число положительным
        /// </summary>
        /// <param name="number">Число, которое требует проверки</param>
        /// <returns>Возвращает true если число положительное</returns>
        public static bool IsPositive(int number)
        {
            if (number > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Метод проверяет, является ли число четным
        /// </summary>
        /// <param name="number">Число, которое требует проверки</param>
        /// <returns>Возвращает true если число четное</returns>
        public static bool IsEven(int number)
        {
            if ((number & 1) == 0)
            {
                return true;
            }

            return false;
        }


    }
}
