
using System;
using System.IO;
using DesignPatterns.Interfaces;
using DesignPatterns.Services.ConcreteFactories;
using DesignPatterns.Services;

class Program
{
    static void Main(string[] args)
    {
        string filePath = @"D://example.txt";
        IFileFactory fileFactory = new TextFileFactory();
        IFileHandler fileHandler = fileFactory.CreateFileHandler(filePath);

        RunMenu(fileHandler);

        ((FileHandler)fileHandler).Close();
    }

    static void RunMenu(IFileHandler fileHandler)
    {
        bool exit = false;

        while (!exit)
        {
            ShowMenu();
            int choice = GetMenuChoice();

            switch (choice)
            {
                case 1:
                    string content = GetContentFromUser();
                    fileHandler.WriteToFile(content);
                    Console.WriteLine("Content written to file.");
                    break;

                case 2:
                    string readContent = fileHandler.ReadFromFile();
                    Console.WriteLine("File Content:\n" + readContent);
                    break;

                case 3:
                    exit = true;
                    Console.WriteLine("Exiting the application.");
                    break;

                default:
                    Console.WriteLine("Invalid choice. Try again.");
                    break;
            }
        }
    }

    static void ShowMenu()
    {
        
        Console.WriteLine("\n===== File Handler Menu =====");
        Console.WriteLine("1. Write to File");
        Console.WriteLine("2. Read from File");
        Console.WriteLine("3. Exit");
        Console.Write("Enter your choice (1-3): ");
    }

    static int GetMenuChoice()
    {
        int choice;
        while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 3)
        {
            Console.Write("Invalid input. Please enter a number between 1 and 3: ");
        }
        return choice;
    }

    static string GetContentFromUser()
    {
        Console.Write("Enter content to write to the file: ");
        return Console.ReadLine() ?? string.Empty;
    }

}