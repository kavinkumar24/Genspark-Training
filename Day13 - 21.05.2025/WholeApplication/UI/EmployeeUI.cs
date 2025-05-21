using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholeApplication.Interfaces;
using WholeApplication.Repositories;
using WholeApplication.Services;
using WholeApplication.Models;

namespace WholeApplication.UI
{
    internal class EmployeeUI
    {
        private readonly IEmployeeService _employeeService;
        public EmployeeUI(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
        public void ShowUsersMenu()
        {
            while (true)
            {
                Console.WriteLine("Menu for Employee Management \n");
                Console.WriteLine("1. Add Employee");
                Console.WriteLine("2. Search Employee");
                Console.WriteLine("3. Exit");
                Console.WriteLine("Please enter your choice");
                int choice;
                while(!int.TryParse(Console.ReadLine(), out  choice))
                {
                    Console.WriteLine("Invalid entry. Please enter a valid choice");
                }

                switch (choice)
                {
                    case 1:
                        AddEmployee();
                        break;
                    case 2:
                        SearchEmployee();
                        break;
                    case 3:
                        return;

                    default:
                        Console.WriteLine("Invalid Option");
                        break;
                }
            }
        }

        private void SearchEmployee()
        {
            Console.WriteLine("Please enter the employee ID");
            int id;
            while (!int.TryParse(Console.ReadLine(), out id) || id <= 0)
            {
                Console.WriteLine("Invalid entry for ID. Please enter a valid employee ID");
            }
            Console.WriteLine("Please enter the employee name");
            string name = Console.ReadLine() ?? "";
            Console.WriteLine("Please enter the employee age");
            int age;
            while (!int.TryParse(Console.ReadLine(), out age) || age <= 18)
            {
                Console.WriteLine("Invalid entry for age. Please enter a valid employee age");
            }
            Console.WriteLine("Please enter the employee salary");
            float salary;
            while (!float.TryParse(Console.ReadLine(), out salary) || salary <= 0)
            {
                Console.WriteLine("Invalid entry for salary. Please enter a valid employee salary");
            }
        }

        private void AddEmployee()
        {
            Employee employee = new Employee();
            employee.TakeEmployeeDetailsFromUser();
            var emp = new Employee
            {
                Name = employee.Name,
                Id = employee.Id,
                Age = employee.Age,
                Salary = employee.Salary
            };
            int id = _employeeService.AddEmployee(emp);
            if (id != -1)
            {
                Console.WriteLine("Employee added successfully with ID: " + id);
            }
            else
            {
                Console.WriteLine("Failed to add employee");
            }
        }

    }
}
