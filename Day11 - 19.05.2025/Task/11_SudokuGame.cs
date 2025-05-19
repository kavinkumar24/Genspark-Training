


using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// 11)  In the question ten extend it to validate a sudoku game. Validate all 9 rows (use int[,] board = new int[9, 9])
namespace CSharpPractice
{
    internal class SudokuGame
    {
        public static int[,] board = new int[9, 9];

        public static void Run()
        {
            Console.WriteLine($"Enter 9 numbers for row, separated by space:");
            for (int row = 0; row < 9; row++)
            {

                string? input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Invalid input. Try again.");
                    row--;
                    continue;
                }

                string[] parts = input.Split(' ');
                if (parts.Length != 9)
                {
                    Console.WriteLine("Exactly 9 numbers required.");
                    row--;
                    continue;
                }

                for (int col = 0; col < 9; col++)
                {
                    if (!int.TryParse(parts[col], out int num) || num < 1 || num > 9)
                    {
                        Console.WriteLine("All numbers must be from 1 to 9.");
                        row--;
                        break;
                    }
                    board[row, col] = num;
                }
            }

            for (int i = 0; i < 9; i++)
            {
                Console.WriteLine($"Row {i + 1}: {(Rowsvalidation(i) ? "Valid" : "Invalid")}");
                Console.WriteLine($"Column {i + 1}: {(Columnsvalidation(i) ? "Valid" : "Invalid")}");
            }

            for (int row = 0; row < 9; row += 3)
            {
                for (int col = 0; col < 9; col += 3)
                {
                    Console.WriteLine($"Subgrid ({row + 1},{col + 1}): {(Subgridsvalidation(row, col) ? "Valid" : "Invalid")}");
                }
            }
        }

        public static bool Rowsvalidation(int row)
        {
            HashSet<int> flag = new HashSet<int>();
            for (int col = 0; col < 9; col++)
            {
                int val = board[row, col];
                if (!flag.Add(val)) return false;
            }
            return true;
        }

        public static bool Columnsvalidation(int col)
        {
            HashSet<int> flag = new HashSet<int>();
            for (int row = 0; row < 9; row++)
            {
                int val = board[row, col];
                if (!flag.Add(val)) return false;
            }
            return true;
        }

        public static bool Subgridsvalidation(int startRow, int startCol)
        {
            HashSet<int> flag = new HashSet<int>();
            for (int row = startRow; row < startRow + 3; row++)
            {
                for (int col = startCol; col < startCol + 3; col++)
                {
                    int val = board[row, col];
                    if (!flag.Add(val)) return false;
                }
            }
            return true;
        }
    }



}
