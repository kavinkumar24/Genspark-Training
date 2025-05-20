using System;
using System.Collections.Generic;

class Employee
{
    public string Name { get; set; } = string.Empty;

    public static List<Employee> employees = new List<Employee>();

    public bool TakeEmployeeDetailsFromUser()
    {
        string? input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
            return false;
        Name = input.Trim();
        return true;
    }

    public override string ToString()
    {
        return "\nEmployee Name: " + Name;
    }

    public static void CollectEmployeeNames()
    {
        Console.WriteLine("Please enter employee names in order of their eligibility for promotion (please enter blank to stop):");

        while (true)
        {
            Employee employee = new Employee();
            bool isValid = employee.TakeEmployeeDetailsFromUser();
            if (!isValid)
                break;

            employees.Add(employee);
        }

        int beforeTrim = employees.Capacity;
        employees.TrimExcess();
        int afterTrim = employees.Capacity;
        Console.WriteLine($"The current size of collection is : {beforeTrim}");
        Console.WriteLine($"The size after removing the extra space is {afterTrim}");
    }

    public static void DisplayAllEmployees()
    {
        Console.WriteLine("\n\nDisplaying Employee Details");
        Console.WriteLine("=====================================");
        foreach (var employee in employees)
        {
            Console.WriteLine(employee.ToString());
        }
    }

    public static void FindPromotionPosition()
    {
        Console.WriteLine("\nPlease enter the name of the employee to check promotion position:");
        string? employeeName = Console.ReadLine()?.Trim();

        if (string.IsNullOrEmpty(employeeName))
        {
            Console.WriteLine("Invalid input. Please enter a valid employee name.");
            return;
        }

        int position = employees.FindIndex(e => e.Name.ToLower().IndexOf(employeeName.ToLower()) >= 0);

        if (position != -1)
        {
            Console.WriteLine($"\"{employeeName}\" is in position {position + 1} for promotion.");
        }
        else
        {
            Console.WriteLine($"Employee \"{employeeName}\" not found in the list.");
        }
    }

    public static void SortedEmployeeList()
    {
        var sortedemployees = employees.OrderBy(e => e.Name).ToList();
        Console.WriteLine("\n\nSorted Employee List");
        foreach (var employee in sortedemployees)
        {
            Console.WriteLine(employee.ToString());
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Employee.CollectEmployeeNames();

        bool running = true;
        while (running)
        {
            Console.WriteLine("\nChoose an option:");
            Console.WriteLine("1. Display all employees eligible for promotion");
            Console.WriteLine("2. Find promotion position of an employee");
            Console.WriteLine("3. Display sorted list of employees");
            Console.WriteLine("4. Exit");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Employee.DisplayAllEmployees();
                    break;
                case "2":
                    Employee.FindPromotionPosition();
                    break;
                case "3":
                    Employee.SortedEmployeeList();
                    break;
                case "4":
                    Console.WriteLine("Exiting the program.");
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please enter a valid option (1, 2, or 3).");
                    break;
            }
        }
    }
}


