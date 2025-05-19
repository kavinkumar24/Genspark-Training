using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CSharpPractice
{
   
    internal class GreetUser
    {
        private static void Greet(string? name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                Console.WriteLine($"Welcome, {name}!");
            }
            else
            {
                Console.WriteLine("Enter valid name");
            }
        }

        public static void Run()
        {
            Console.WriteLine("Enter your name:");
            string? name = Console.ReadLine();
            Greet(name);
        }   
    }
}
