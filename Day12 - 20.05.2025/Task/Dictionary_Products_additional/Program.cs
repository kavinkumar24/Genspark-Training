



//Create a Dictionary<string, double> where the key is the product name and value is the price.

//Add 5 products

//Display all key-value pairs

//Search for a specific product and show its price

//Expected Concepts:

//Working with Dictionary<string, double>

//Searching with ContainsKey

using System.Diagnostics;

class Program
{
    public static (string, double) GetProductsInputFromUser()
    {
        Console.WriteLine("Enter the Product name (unique)");
        string? input = Console.ReadLine();
        Console.WriteLine($"Enter the Price for {input}");
        double result;
        while (!double.TryParse(Console.ReadLine(), out result))
        {
            Console.WriteLine("Please ensure the format");
        }
        return (input.Trim(), result);
    }

    public static void DisplayProducts(Dictionary<string, double> products)
    {
        foreach (var entry in products)
        {
            Console.WriteLine(entry.Key + " " + entry.Value);
        }
    }

    public static void SearchProducts(Dictionary<string, double> products)
    {
        Console.WriteLine("Enter Product name to search\n");
        string? searchName = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(searchName) && products.TryGetValue(searchName, out double price))
        {
            Console.WriteLine($"Price of the {searchName} is Rs.{price}");
        }
        else
        {
            Console.WriteLine("Not found");
        }
    }

    static void Main(string[] args)
    {
        Dictionary<string, double> products = new Dictionary<string, double>();
        bool isRun = true;

        while (isRun)
        {
            var (ProductName, ProductPrice) = GetProductsInputFromUser();

            if (products.ContainsKey(ProductName))
            {
                Console.WriteLine("Products already exists");
            }
            else
            {
                products.Add(ProductName, ProductPrice);
                Console.WriteLine("Products added");
            }
            Console.WriteLine("Add another? (y/n)");
            string? symbol = Console.ReadLine();
            if (symbol == "y")
            {
                isRun = true;
            }
            else if (symbol == "n")
            {
                isRun = false;
            }
            else
            {
                Console.WriteLine("Invalid format");
            }
        }

        DisplayProducts(products);

        SearchProducts(products);
    }
}
