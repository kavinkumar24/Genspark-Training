using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//8) Given two integer arrays, merge them into a single array.
//Input: { 1, 3, 5}
//and {2, 4, 6}
//Output: { 1, 3, 5, 2, 4, 6}
using System;

namespace CSharpPractice
{
    internal class TwoArrayMerge
    {
        public static void MergeArrays(int[] arr1, int[] arr2)
        {
            int n = arr1.Length;
            int m = arr2.Length;
            int[] mergedArray = new int[n + m];

            int k = 0;

            for (int i = 0; i < n; i++)
            {
                mergedArray[k++] = arr1[i];
            }

            for (int j = 0; j < m; j++)
            {
                mergedArray[k++] = arr2[j];
            }

            Console.WriteLine("Merged Array:");
            foreach (var item in mergedArray)
            {
                Console.Write(item + " ");
            }
        }

        public static void Run()
        {
            Console.Write("Please Enter the number of elements n: ");
            string? input = Console.ReadLine();

            if (!int.TryParse(input, out int n) || n <= 0)
            {
                Console.WriteLine("Invalid size.");
                return;
            }

            int[]? arr1 = Program.GetArrayFromUser(n);

            Console.Write("Please Enter the number of elements m: ");

            if (!int.TryParse(Console.ReadLine(), out int m) || m <= 0)
            {
                Console.WriteLine("Invalid size.");
                return;
            }

            int[]? arr2 = Program.GetArrayFromUser(m);

            if (arr1 == null || arr2 == null)
            {
                Console.WriteLine("One or both arrays are null. Cannot merge.");
                return;
            }

            MergeArrays(arr1, arr2);
        }
    }
}


