using System;
using System.Collections.Generic;
using System.Linq;

class Employee : IComparable<Employee>
{
    int id;
    int age;
    double salary;

    public int Id { get => id; set => id = value; }
    public string Name { get; set; } = string.Empty;
    public int Age { get => age; set => age = value; }
    public double Salary { get => salary; set => salary = value; }

    private static Dictionary<int, Employee> employeeDict = new Dictionary<int, Employee>();

    public void TakeEmployeeDetailsFromUser()
    {
        Console.WriteLine("Please enter Employee ID:");
        while (true)
        {
            if (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Please enter a valid integer ID:");
                continue;
            }

            if (employeeDict.ContainsKey(id))
            {
                Console.WriteLine($"Employee ID {id} already exists. Please enter a unique ID:");
                continue;
            }

            Id = id;
            break;
        }

        Console.WriteLine("Please enter employee name:");
        string? name = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Please enter a valid name:");
            name = Console.ReadLine();
        }
        Name = name.Trim();

        Console.WriteLine("Please enter employee age:");
        while (!int.TryParse(Console.ReadLine(), out age) || age < 0)
            Console.WriteLine("Plase enter a valid age:");
        Age = age;

        Console.WriteLine("Please enter employee salary:");
        while (!double.TryParse(Console.ReadLine(), out salary) || salary < 0)
            Console.WriteLine("Please enter a valid salary:");
        Salary = salary;

    }


    public override string ToString()
    {
        return $"Employees Detials \n ID: {Id}, Name: {Name}, Age: {Age}, Salary: {Salary}";
    }

    public int CompareTo(Employee others)
    {
        return this.Salary.CompareTo(others.Salary);
    }



    public static void AddEmployee()    // Medium 1
    {
        Employee emp = new Employee();
        emp.TakeEmployeeDetailsFromUser();

        if (employeeDict.ContainsKey(emp.Id))
        {
            Console.WriteLine($"Employee ID {emp.Id} already exists.");
            return;
        }

        employeeDict[emp.Id] = emp;
        Console.WriteLine("-----Employee added.-----");
    }

    public static void SortEmployeesBySalary()   // Medium 2
    {
        List<Employee> empList = employeeDict.Values.ToList();
        empList.Sort();

        Console.WriteLine("\nEmployees sorted by salary:");
        foreach (var emp in empList)
        {
            Console.WriteLine(emp.ToString());
        }
    }

    public static void GetEmployeeById()   // Medium 2
    {
        Console.Write("Please enter employee ID: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {

            var emp = employeeDict.Values.Where(e=>e.Id == id).FirstOrDefault();

            if(emp!=null) Console.WriteLine($"Employee Details:\n{emp}");
            else Console.WriteLine("Employee not found.");
        }
        else
        {
            Console.WriteLine("Invalid Id");
        }
    }

  

    public static void FindEmployeesByName()   // Medium 3
    {
        Console.Write("Please enter name to search: ");
        string? name = Console.ReadLine()?.Trim().ToLower();

        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Invalid name.");
            return;
        }

        var result = employeeDict.Values
                        .Where(e => e.Name.ToLower() == name)
                        .ToList();

        if (result.Any())
        {
            Console.WriteLine("Matching employees:");
            result.ForEach(e => Console.WriteLine(e));
        }
        else
        {
            Console.WriteLine("No employees found with that name.");
        }
    }

    public static void FindElderEmployees()  // Medium 4
    {
        Console.Write("Please enter employee ID to compare age: ");
        if (!int.TryParse(Console.ReadLine(), out int id) || !employeeDict.ContainsKey(id))
        {
            Console.WriteLine("Invalid employee ID.");
            return;
        }

        var baseEmployee = employeeDict[id];
        var elderEmployees = employeeDict.Values
                                .Where(e => e.Age > baseEmployee.Age)
                                .ToList();

        if (elderEmployees.Any())
        {
            Console.WriteLine($"Employees older than {baseEmployee.Name} (Age: {baseEmployee.Age}):");
            elderEmployees.ForEach(e => Console.WriteLine(e));
        }
        else
        {
            Console.WriteLine("No employees are older than the given employee.");
        }
    }
    public static void PrintAllEmployees()
    {
        if(employeeDict.Count == 0)
        {
            Console.WriteLine("----No employees found.----");
            return;
        }
        Console.WriteLine("\nAll Employees:");
        foreach (var emp in employeeDict.Values)
        {
            Console.WriteLine(emp.ToString());
        }
    }

    private static string GetUpdatedName()
    {
        Console.WriteLine("Please enter new name");
        string? name = Console.ReadLine();
        return string.IsNullOrWhiteSpace(name) ? string.Empty : name.Trim();
    }

    private static int GetUpdatedAge()
    {
        int newAge;
        Console.WriteLine("Please enter new age");
        while (!int.TryParse(Console.ReadLine(), out newAge)|| newAge<0)
            Console.WriteLine("Please enter a valid age:");
        return newAge;
    }
    
    private static double GetUpdatedSalary()
    {
        double newSalary;
        Console.WriteLine("Please enter new salary");
        while (!double.TryParse(Console.ReadLine(), out newSalary) || newSalary<0)
            Console.WriteLine("Please enter a valid salary:");
        return newSalary;
    }


    public static void UpdateEmployeeDetails()
    {
        Console.Write("Please enter employee ID to update: ");

        if (int.TryParse(Console.ReadLine(), out int id))
        {
            if (employeeDict.TryGetValue(id, out Employee emp))
            {
                Console.WriteLine($"Current Details: {emp}");

                Console.WriteLine("\nWhat do you want to update?");
                Console.WriteLine("1. Name");
                Console.WriteLine("2. Age");
                Console.WriteLine("3. Salary");
                Console.Write("Please enter your choice: ");
                string? choice = Console.ReadLine();

                if (choice == "1")
                {
                    string newName = GetUpdatedName();
                    if (!string.IsNullOrEmpty(newName))
                    {
                        emp.Name = newName;
                        Console.WriteLine("----------Name updated----------.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid name. No changes made.");
                    }
                }
                else if (choice == "2")
                {
                    int newAge = GetUpdatedAge();
                    if (newAge > 0)
                    {
                        emp.Age = newAge;
                        Console.WriteLine("-----Age updated.----");
                    }
                    else
                    {
                        Console.WriteLine("Invalid age. No changes made.");
                    }
                }
                else if (choice == "3")
                {
                    double newSalary = GetUpdatedSalary();
                    if (newSalary >= 0)
                    {
                        emp.Salary = newSalary;
                        Console.WriteLine("----Salary updated.----");
                    }
                    else
                    {
                        Console.WriteLine("Invalid salary. No changes made.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid option selected.");
                }
            }
            else
            {
                Console.WriteLine("Employee with that ID not found.");
            }
        }
        else
        {
            Console.WriteLine("Invalid ID.");
        }
    }

    public static void DeleteEmployee(Employee emp)
    {
        int id;
        Console.Write("Please enter employee ID to delete: ");

        while(!int.TryParse(Console.ReadLine(), out id))
        {
            Console.WriteLine("Please enter a valid integer ID:");
        }

        if (employeeDict.ContainsKey(id))
        {
            employeeDict.Remove(id);
            Console.WriteLine($"-----Employee with ID {id} deleted.-----");
        }
        else
        {
            Console.WriteLine($"Employee with ID {id} not found.");
        }

    }

}



class Program
{
    static void Main(string[] args)
    {
        bool exit = false;

        while (!exit)
        {
            Console.WriteLine("\nMenu:");
            Console.WriteLine("1. Add Employee");
            Console.WriteLine("2. Get Employee by ID");
            Console.WriteLine("3. Sort Employees by Salary");
            Console.WriteLine("4. Find Employees by Name");
            Console.WriteLine("5. Find Employees Older Than a Given Employee");
            Console.WriteLine("6. Get All Employees");
            Console.WriteLine("7. Update Employee Details");
            Console.WriteLine("8. Delete Employee");
            Console.WriteLine("9. Exit");
            Console.Write("Choose an option: ");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Employee.AddEmployee();
                    break;
                case "2":
                    Employee.GetEmployeeById();
                    break;
                case "3":
                    Employee.SortEmployeesBySalary();
                    break;
                case "4":
                    Employee.FindEmployeesByName();
                    break;
                case "5":
                    Employee.FindElderEmployees();
                    break;
                case "6":
                    Employee.PrintAllEmployees();
                    break;
                case "7":
                    Employee.UpdateEmployeeDetails();
                    break;
                case "8":
                    Employee emp = new Employee();
                    Employee.DeleteEmployee(emp);
                    break;
                case "9":
                    exit = true;
                    Console.WriteLine("Exiting...");
                    break;
                default:
                    Console.WriteLine("Invalid option. Try again.");
                    break;
            }
        }
    }
}


