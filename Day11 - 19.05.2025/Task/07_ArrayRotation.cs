using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//7) create a program to rotate the array to the left by one position.
//Input: { 10, 20, 30, 40, 50}
//Output: { 20, 30, 40, 50, 10}

namespace CSharpPractice
{
    internal class ArrayRotation
    {
        public static void Run()
        {
            Console.Write("Enter the number of elements: ");
            string? input = Console.ReadLine();

            if (!int.TryParse(input, out int n) || n <= 0)
            {
                Console.WriteLine("Invalid size.");
                return;
            }

            int[]? numbers = Program.GetArrayFromUser(n);
            if (numbers == null)
                return;

            RotateLeftOne(numbers);

            foreach(int num in numbers)
            {
                Console.Write(num+" ");
            }
        }

        public static void RotateLeftOne(int[] numbers) 
        {
            if (numbers.Length <= 1) return;
            int firstElement = numbers[0];

            for (int i = 0; i < numbers.Length-1; i++)
            {
                numbers[i] = numbers[i + 1];
            }
            numbers[numbers.Length - 1] = firstElement;
        }

    }

}