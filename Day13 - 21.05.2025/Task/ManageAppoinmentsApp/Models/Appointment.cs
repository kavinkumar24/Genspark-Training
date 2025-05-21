using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManageAppoinmentsApp.Helper;

namespace ManageAppoinmentsApp.Models
{ 
    //: IComparable<Appointment>
    public class Appointment 
    {
        public int Id { get; set; }
        public string? PatientName { get; set; } = string.Empty;
        public int PatientAge { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Reason { get; set; } = string.Empty;

        public Appointment(int id, string patientName, int patientAge, DateTime appointmentDate, string reason)
        {
            Id = id;
            PatientName = patientName;
            PatientAge = patientAge;
            AppointmentDate = appointmentDate;
            Reason = reason;
        }

        public Appointment()
        {
        }

        public void TakeAppointmentDetailsFromUser()
        {
            Console.WriteLine("Please enter Patient Name:");
            int age;
            DateTime date;  
            PatientName = Console.ReadLine();
            if (string.IsNullOrEmpty(PatientName))
            {
                Console.WriteLine("Patient Name cannot be empty.");
                PatientName = Console.ReadLine();
            }

            Console.WriteLine("Please enter Patient Age:");
            while (!int.TryParse(Console.ReadLine(), out age) || age <= 0)
            {
                Console.WriteLine("Invalid input. Please enter a valid age.");
            }
            PatientAge = age;
            Console.WriteLine("Please enter Appointment Date (yyyy-mm-dd HH:SS):");
            while (true)
            {
                string dateInput = Console.ReadLine();
                if (DateTime.TryParse(dateInput, out date))
                {
                    if(DateHelper.isValidDate(date.Date))
                    break;

                    else
                    {
                        Console.WriteLine("Date can't be in past");
                        Console.WriteLine($"-----Today Date is {DateTime.Now}----");
                        Console.WriteLine("Please enter correct date");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid date format");
                }
            }
            AppointmentDate = date;
            Console.WriteLine("Please enter Reason for Appointment:");
            Reason = Console.ReadLine();
            if (string.IsNullOrEmpty(Reason))
            {
                Console.WriteLine("Reason cannot be empty.");
                Reason = Console.ReadLine();
            }
        }

        public override string ToString()
        {
            return $"Id: {Id}, Patient Name: {PatientName}, Patient Age: {PatientAge}, Appointment Date: {AppointmentDate}, Reason: {Reason}";
        }

        //public int CompareTo(Appointment others)
        //{
        //    return this.AppointmentDate.CompareTo(others.AppointmentDate);
        //}
    }
}
