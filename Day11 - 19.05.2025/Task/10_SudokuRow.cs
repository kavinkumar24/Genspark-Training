using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 10) write a program that accepts a 9-element array representing a Sudoku row.
namespace CSharpPractice
{
    internal class SudokuRow
    {
        public static int[]? GetSudokuRow()
        {
            int rowLength = 9;
            Console.WriteLine("Please Enter 9 numbers, seperated by space");
            string? elements = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(elements))
            {
                Console.WriteLine("No input provided.");
                return null;
            }

            string[] parts = elements.Split(' ');
            if (parts.Length != 9)
            {
                Console.WriteLine("Invalid input, please enter exactly 9 numbers");
                return null;
            }
            int[] row = new int[rowLength];
            for (int i = 0; i < rowLength; i++)
            {
                if (!int.TryParse(parts[i], out row[i]) || row[i] < 1 || row[i] > 9)
                {
                    Console.WriteLine("Please enter the number that are in 1 to 9");
                    return null;
                }
            }
            return row;
        }

        public static bool IsvalidSudokuRow(int[] row)
        {
            HashSet<int> flag = new HashSet<int>();
            foreach (int num in row)
            {
                if (flag.Contains(num))
                {
                    return false;
                }
                flag.Add(num);
            }
            return true;
        }

        public static void Run()
        {
            int[]? row = GetSudokuRow();
            if (row == null)
            {
                Console.WriteLine("Invalid Input");
                return;
            }
            bool isValidRow = IsvalidSudokuRow(row);

            if (isValidRow)
            {
                Console.WriteLine("The sudoku row is valid");
            }
            else
            {
                Console.WriteLine("The sudoku row is invalid");
            }
        }
    }
}
