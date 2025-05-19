using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//4) Take username and password from user. Check if user name is "Admin" and password is "pass"
//if yes then print success message.
//Give 3 attempts to user. In the end of eh 3rd attempt if user still is unable to provide valid
//creds then exit the application after print the message 
//"Invalid attempts for 3 times. Exiting...."
namespace CSharpPractice
{
    using System;

    internal class Validation
    {
        private static void GetStoredCredentials(out string username, out string password)
        {
            username = "Admin";
            password = "pass";
        }

        private static void GetUserInput(out string? inputUsername, out string? inputPassword)
        {
            Console.WriteLine("Enter username:");
            inputUsername = Console.ReadLine();

            Console.WriteLine("Enter password:");
            inputPassword = Console.ReadLine();
        }

        private static bool ValidateUserCredentials(string? inputUsername, string? inputPassword, 
            string storedUsername, string storedPassword)
        {
            return string.Equals(inputUsername, storedUsername) && string.Equals(inputPassword, storedPassword);
        }

        public static void Run()
        {
            GetStoredCredentials(out string storedUsername, out string storedPassword);
            int attempts = 0;

            while (attempts < 3)
            {
                GetUserInput(out string? inputUsername, out string? inputPassword);

                if (ValidateUserCredentials(inputUsername, inputPassword, storedUsername, storedPassword))
                {
                    Console.WriteLine("Login successful!");
                    return;
                }

                attempts++;
                Console.WriteLine($"Invalid credentials. You have {3 - attempts} attempts left.");
            }

            Console.WriteLine("Invalid attempts for 3 times. Exiting....");
        }
    }




}
