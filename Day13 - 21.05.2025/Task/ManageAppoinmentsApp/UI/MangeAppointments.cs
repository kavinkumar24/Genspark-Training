using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManageAppoinmentsApp.Interface;
using ManageAppoinmentsApp.Models;

namespace ManageAppoinmentsApp.UI
{
    public class MangeAppointments
    {
        private readonly IAppointmentService _appointmentService;


        public MangeAppointments(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        public void ShowMenuOptions()
        {
            Console.WriteLine("1. Add Appointment");
            Console.WriteLine("2. Search Appointments");
            Console.WriteLine("3. Sort Appointments");
            Console.WriteLine("4. Exit");
            Console.Write("Please select an option: ");
        }
        public void Run()
        {
            int choice;
            Console.WriteLine("Welcome to the Appointment Management System!");
            Console.WriteLine("--------------------------------------------");
            while (true)
            {
                ShowMenuOptions();
                while(!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 4)
                {
                    Console.WriteLine("Invalid input. Please enter a number between 1 and 3.");
                }
                switch (choice)
                {
                    case 1:
                        AddAppointment();
                        break;
                    case 2:
                        SearchAppointments();
                        break;
                    case 3:
                        SortAppointments();
                        break;
                    case 4:
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;

                }
            }
        }

        private void AddAppointment()
        {
            Appointment appointment = new Appointment();
            appointment.TakeAppointmentDetailsFromUser();
            int id = _appointmentService.AddAppointment(appointment);
            Console.WriteLine($"----------Appointment added successfully with ID: {id} ---------\n");

        }

        private void SortAppointments()
        {
            var sortModel = GetSortModel();
            if (sortModel == null) return;

            var appointments = _appointmentService.SortByAppointments(sortModel);
            if (appointments == null || appointments.Count == 0)
            {
                Console.WriteLine("No Appointments to show.");
                return;
            }

            PrintAppointmentDetails(appointments);
        }


        private static SortModel GetSortModel()
        {
            Console.WriteLine("Sort Appointments By:");
            Console.WriteLine("1. Appointment ID");
            Console.WriteLine("2. Patient Name");
            Console.WriteLine("3. Patient Age");
            Console.WriteLine("4. Appointment Date");
            Console.WriteLine("5. Exit");
            Console.Write("Select an option: ");

            int choice;
            while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 5)
            {
                Console.WriteLine("Invalid input. Please enter a number between 1 and 5.");
            }

            if (choice == 5)
                return null;

            return new SortModel { SortOption = choice };
        }

        private void SearchAppointments()
        {
            var searchMenu = ShowSearchMenuOptions();
            var appointments = _appointmentService.SearchAppointments(searchMenu);
            Console.WriteLine("The search options you have selected");
            Console.WriteLine(searchMenu);
            if ((appointments == null))
            {
                Console.WriteLine("No Appointments for the search");
            }
            PrintAppointmentDetails(appointments);
        }

        private void PrintAppointmentDetails(List<Appointment> appointments)
        {
            Console.WriteLine("Appointment details");
            Console.WriteLine("-------------------");
            if(appointments == null)
            {
                Console.WriteLine("No Appointments to show.");
                return;
            }
            foreach (var appointment in appointments)
            {
                Console.WriteLine(appointment.ToString());
            }
        }
        private SearchModel ShowSearchMenuOptions()
        {
            SearchModel searchModel = new SearchModel();
            Console.WriteLine("1. Search By Appointment ID (starts from 1)");
            Console.WriteLine("2. Search by Patient Name");
            Console.WriteLine("3. Search by Patient Age");
            Console.WriteLine("4. Search by Appointment Date");
            Console.WriteLine("5. Exit");
            Console.Write("Please select an option: ");
            int choice;
            while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 5)
            {
                Console.WriteLine("Invalid input. Please enter a number between 1 and 5.");
            }
            switch (choice)
            {
                case 1:
                    Console.Write("Please enter appointment ID: ");
                    int id = 0;
                    while (!int.TryParse(Console.ReadLine(), out id) || (id < 1))
                    {
                        Console.WriteLine("Invalid entry. Please enter the appointment ID starting from 1.");
                    }
                    searchModel.Id = id; 
                    break;
                case 2:
                    Console.Write("Please enter patient name: ");
                    searchModel.PatientName = Console.ReadLine();
                    break;
                case 3:
                    Console.Write("please enter patient age: ");
                    
                    searchModel.PatientAge = new Range<int>();
                    int age;
                    Console.WriteLine("please enter the min employee age");
                    while (!int.TryParse(Console.ReadLine(), out age) || age <= 18)
                    {
                        Console.WriteLine("Invalid entry for min age. Please enter a valid employee age");
                    }
                    searchModel.PatientAge.MinVal = age;
                    Console.WriteLine("Please enter the max employee Age");
                    while (!int.TryParse(Console.ReadLine(), out age) || age <= 18)
                    {
                        Console.WriteLine("Invalid entry for max age. Please enter a valid employee age");
                    }
                    searchModel.PatientAge.MaxVal = age;
                    break;
                case 4:
                    Console.Write("please enter appointment date (yyyy-mm-dd): ");
                    DateTime date;
                    while (!DateTime.TryParse(Console.ReadLine(), out date))
                    {
                        Console.WriteLine("Invalid date format. Please enter a valid date in yyyy-mm-dd format.");
                    }
                    searchModel.AppointmentDate = date;
                    break;
                case 5:
                    return null;
                default:
                    Console.WriteLine("Invalid");
                    break;
            }
            return searchModel;
        }

    
    }
}
