using System;

class Program
    {
    public static (string[][], int[][]) GetUserInput()
    {
        int noOfPersons = GetInteger("Enter number of persons:");
        string[][] postCaptions = new string[noOfPersons][];
        int[][] postLikes = new int[noOfPersons][];

        for (int i = 0; i < noOfPersons; i++)
        {
            Console.WriteLine($"\nUser {i + 1}:");
            int noOfPosts = GetInteger("How many posts?");
            postCaptions[i] = new string[noOfPosts];
            postLikes[i] = new int[noOfPosts];

            for (int j = 0; j < noOfPosts; j++)
            {
                postCaptions[i][j] = GetString($"Enter caption for post {j + 1}:");
                postLikes[i][j] = GetInteger($"Enter likes for post {j + 1}:");
            }
        }

        return (postCaptions, postLikes);
    }

    public static int GetInteger(string message)
    {
        int value;
        Console.WriteLine(message);
        while (!int.TryParse(Console.ReadLine(), out value) || value <= 0)
        {
            Console.WriteLine("Please enter a valid positive integer.");
        }
        return value;
    }

    public static string GetString(string message)
    {
        Console.WriteLine(message);
        string? input = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("Input cannot be empty. Try again:");
            input = Console.ReadLine();
        }
        return input;
    }

    public static void DisplayPosts(string[][] captions, int[][] likes)
    {
        Console.WriteLine("\n Displaying Instagram Posts\n");
        Console.WriteLine("=====================================");

        for (int i = 0; i < captions.Length; i++)
        {
            Console.WriteLine($"User {i + 1}:");
            for (int j = 0; j < captions[i].Length; j++)
            {
                Console.WriteLine($"Post {j + 1} - Caption: {captions[i][j]} | Likes: {likes[i][j]}");
            }
            Console.WriteLine();
        }
    }

    static void Main(string[] args)
        {
        var (captions, likes) = GetUserInput();
        DisplayPosts(captions, likes);
    }

    }

