using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//6) Count the Frequency of Each Element
//Given an array, count the frequency of each element and print the result.
//Input: { 1, 2, 2, 3, 4, 4, 4}

//output
//1 occurs 1 times  
//2 occurs 2 times  
//3 occurs 1 times  
//4 occurs 3 times
namespace CSharpPractice
{
    internal class NumberFrequency
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

            Dictionary<int, int> frequency = new Dictionary<int, int>();
            foreach (int num in numbers)
            {
                if (frequency.ContainsKey(num))
                    frequency[num]++;
                else
                    frequency[num] = 1;
            }

            Console.WriteLine("Frequencies of each number:");
            foreach (var entry in frequency)
            {
                Console.WriteLine($"{entry.Key} occurs {entry.Value} times");
            }
        }

    }
}

