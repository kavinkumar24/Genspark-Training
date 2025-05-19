using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpPractice
{
    internal class LargestOfTwoNumbers
    {
        private static void LargestNumber(int num1, int num2)
        {
            if (num1 == num2) Console.WriteLine("Equal numbers");
            else if (num1 > num2) Console.WriteLine($"Largest Number is: {num1}");
            else Console.WriteLine($"Largest Number is: {num2}");
        }
        public static void Run() 
        {
            Console.WriteLine("Enter the first number");
            bool valid1 = int.TryParse(Console.ReadLine(), out int num1);
            Console.WriteLine("Enter the second number");
            bool valid2 = int.TryParse(Console.ReadLine(), out int num2);
            if (valid1 && valid2)
                LargestNumber(num1, num2);
            else
                Console.WriteLine("Please check the number format");
        }

    }
}
