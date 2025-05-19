using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

// 12. Encryption and decryption
namespace CSharpPractice
{
    internal class EncryptionDecryption
    {

        public static bool IsValid(string input)
        {
            return Regex.IsMatch(input, @"^[a-z]+$");
        }

        public static string OnEncrypt(string input)
        {
            char[] encrypted = new char[input.Length];
            for(int i=0;i<input.Length; i++)
            {
                char c = input[i];
                encrypted[i] = (char)((c - 'a' + 3) % 26 + 'a');
            }
            return new string(encrypted);
        }

        public static string OnDecrypt(string input)
        {
            char[] encrypted = new char[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                encrypted[i] = (char)((c - 'a' - 3 + 26) % 26 + 'a');
            }
            return new string(encrypted);
        }

        public static void Run()
        {
            Console.WriteLine("Please enter a message for encryption and decryption (only lowercase, no spaces, no symnols)");
            string? input = Console.ReadLine()?.ToLower();
            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("Invalid format");
            }
            else
            {
                if (!IsValid(input))
                {
                    Console.WriteLine("Invalid Input, only lowercase, no spaces, numbers, symbols");
                }
                string encryptedData = OnEncrypt(input);
                string decryptedData = OnDecrypt(encryptedData);

                Console.WriteLine($"Input:{input}");
                Console.WriteLine($"Encrypted:{encryptedData}");
                Console.WriteLine($"Decrypted:{decryptedData}");

            }
        }
    }
}
