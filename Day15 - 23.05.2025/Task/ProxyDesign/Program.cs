using ProxyDesign.Interfaces;
using ProxyDesign.Models;
using ProxyDesign.Service;
using ProxyDesign.Data;
using System;
using System.Collections.Generic;


class Program
{
    static void Main(string[] args)
    {
        Console.Write("Please enter your username: ");
        string inputName = Console.ReadLine()?.Trim() ?? "";
        Console.Write("Please enter your role (Admin/User/Guest): ");
        string inputRole = Console.ReadLine()?.Trim() ?? "";

        User? user = Users.GetUserByName(inputName, inputRole);

        if (user == null)
        {
            Console.WriteLine("Sorry, your are not registered, please come back once you register Correctly");
            return;
        }


        string absoluteFilePath = @"D:\example.txt";
        IFile file = new ProxyFile(absoluteFilePath, user);
        file.Read();
    }


    
}
