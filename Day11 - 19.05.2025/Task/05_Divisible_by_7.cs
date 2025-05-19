using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//5) Take 10 numbers from user and print the number of numbers that are divisible by 7
namespace CSharpPractice
{
    internal class Divisible_by_7
    {

        private static bool IsDivisibleBy7(int number)
        {
            return number % 7 == 0;
        }

        public static void Run()
        {
            Console.WriteLine("Enter 10 numbers:");
            int count = 0;
            for (int i = 0; i < 10; i++)
            {
                if (int.TryParse(Console.ReadLine(), out int number))
                {
                    if (IsDivisibleBy7(number))
                    {
                        count++;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid number.");
                    i--;
                }
            }
            Console.WriteLine($"Number of numbers that are divisible by 7: {count}");
        }
    }
}
