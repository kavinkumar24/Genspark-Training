
using ConsoleApp1;
using CSharpPractice;

class Program
{

    public static int[]? GetArrayFromUser(int size)
    {
        Console.WriteLine($"Enter {size} numbers separated by spaces:");
        string? elements = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(elements))
        {
            Console.WriteLine("No input provided.");
            return null;
        }

        string[] parts = elements.Split();

        if (parts.Length != size)
        {
            Console.WriteLine($"Expected {size} numbers but got {parts.Length}.");
            return null;
        }

        int[] numbers = new int[size];
        for (int i = 0; i < size; i++)
        {
            if (!int.TryParse(parts[i], out numbers[i]))
            {
                Console.WriteLine($"Invalid number at position {i + 1}.");
                return null;
            }
        }

        return numbers;
    }

    static void Main(string[] args)
    {
        GreetUser.Run();

         LargestOfTwoNumbers.Run();

        ArithmeticOperations.Run();

        Validation.Run();

        Divisible_by_7.Run();

        NumberFrequency.Run();

        ArrayRotation.Run();

        TwoArrayMerge.Run();

        BullsCows.Run();

        SudokuRow.Run();

        SudokuGame.Run();

        EncryptionDecryption.Run();

    }
}

