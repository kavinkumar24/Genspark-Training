using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Take 2 numbers from user, check the operation user wants to perform (+,-,*,/).
//Do the operation and print the result

namespace ConsoleApp1
{
    internal class ArithmeticOperations
    {
        public static void PerformOperations(int num1, int num2, string? ops)
        {
            if (string.IsNullOrEmpty(ops))
            {
                Console.WriteLine("Please enter a valid operation.");
                return;
            }
            else if (ops == "+") Console.WriteLine($"Addition of {num1} and {num2} is {checked(num1 + num2)}");

            else if (ops == "-") Console.WriteLine($"Subtraction of {num1} and {num2} is {num1 - num2}");

            else if (ops == "*") Console.WriteLine($"Multiply of {num1} and {num2} is {num1 * num2}");

            else if (ops == "/")
            {
                if(num2 == 0)
                {
                    Console.WriteLine("Division by zero is not allowed.");
                    return;
                }
                else
                    Console.WriteLine($"Division of {num1} and {num2} is {num1 / num2}");
            }

            else Console.WriteLine("Please enter a valid operation.");

        }
         public static void Run()
        {
            Console.WriteLine("Enter the first number");
            bool valid1 = int.TryParse(Console.ReadLine(), out int num1);
            Console.WriteLine("Enter the second number");
            bool valid2 = int.TryParse(Console.ReadLine(), out int num2);
            Console.WriteLine("Enter the operations want to perform (+,-,*,/).");
            string? operation = Console.ReadLine();
            if (valid1 && valid2)
            {
                PerformOperations(num1, num2, operation);
            }

        }  
    }
}
