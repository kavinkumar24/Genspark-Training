using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 9. No of bulls and cows by guessing a word with secret letters
namespace CSharpPractice
{
    internal class BullsCows
    {
        private static string GetSecret()
        {
            string secret = "GAME";
            return secret;
        }

        public static string GetUserString()
        {
            string? input = Console.ReadLine()?.ToUpper();
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Please enter valid string");
                return string.Empty;
            }
                
            else
                return input;   
        }
        public static bool IsAllLetters(string input)
        {
            foreach (char c in input)
            {
                if (!char.IsLetter(c))
                    return false;
            }
            return true;
        }

        public static void Run()
        {
            int attempts = 0;
            string secretWord = GetSecret();

            while (true)
            {
                Console.WriteLine("Please enter the word you guess (alphabet only) - Length 4");
                string guessWord = GetUserString();

                if(guessWord.Length !=secretWord.Length || !IsAllLetters(guessWord))
                {
                    Console.WriteLine("Invalid input");
                    continue;
                }
                attempts++;
                int bulls = 0;
                int cows = 0;
                bool[] secretDone = new bool[secretWord.Length];
                bool[] guessDone = new bool[secretWord.Length];
                List<char> correctChar = new List<char>();
                List<char> misplacedChar = new List<char>();


                for (int i = 0; i < secretWord.Length; i++)
                {
                    if (guessWord[i] == secretWord[i])
                    {
                        bulls++;
                        correctChar.Add(secretWord[i]);
                        secretDone[i] = true;
                        guessDone[i] = true;

                    }
                }
                for (int i = 0; i < secretWord.Length; i++)
                {
                    if (!guessDone[i])
                    {
                        for (int j = 0; j < secretWord.Length; j++)
                        {
                            if (!secretDone[j] && guessWord[i] == secretWord[j])
                            {
                                cows++;
                                misplacedChar.Add(secretWord[j]);
                                secretDone[j] = true;
                                break;
                            }
                        }
                    }
                }

                Console.WriteLine($"{bulls} Bulls, {cows} Cows");
                if( correctChar.Count > 0 )
                {
                    foreach (char c in correctChar)
                    {
                        Console.Write(c + " ");
                    }
                    Console.Write("Correct Position");
                }
                Console.WriteLine();

                if( misplacedChar.Count > 0 )
                {

                    foreach(char c in misplacedChar)
                    {
                        Console.Write(c + " ");
                    }
                    Console.Write(" misplaced");
                }
                Console.WriteLine();

                if (bulls == secretWord.Length)
                {
                    Console.WriteLine($"Super! You guessed the word in {attempts} attempts.");
                    break;
                }

            }
        }

    }
}
