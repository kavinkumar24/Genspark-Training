using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManageAppoinmentsApp.Interface;
using ManageAppoinmentsApp.Models;

namespace ManageAppoinmentsApp.Services
{
    public class AppointmentService : IAppointmentService
    {
        IRepository<int, Models.Appointment> _appointmentRepository;
        public AppointmentService(IRepository<int, Appointment> appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public int AddAppointment(Appointment appointment)
        {
            try
            {
                var addedAppointment = _appointmentRepository.Add(appointment);
                return addedAppointment.Id;
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding appointment: " + ex.Message);
            }
        }
       
        public List<Appointment>? SortByAppointments(SortModel sortModel)
        {
            try
            {
                var appointments = _appointmentRepository.GetAll().ToList();

                switch (sortModel.SortOption)
                {
                    case 1:
                        sortById(appointments);
                        break;
                    case 2:
                        sortByPatientName(appointments);
                        break;
                    case 3:
                        sortByPatientAge(appointments);
                        break;
                    case 4:
                        sortByAppointmentDate(appointments);
                        break;
                    default:
                        break;
                }

                return appointments;
            }
            catch (Exception ex)
            {
                throw new Exception("Error sorting appointments: " + ex.Message);
            }
        }

        private void sortById(ICollection<Appointment> appointments)
        {
            var sortedAppointments = appointments.OrderBy(a => a.Id).ToList();
            foreach (var app in sortedAppointments)
            {
                Console.WriteLine(app.ToString());
            }
        }

        private void sortByPatientName(ICollection<Appointment> appointments)
        {
            var sortedAppointments = appointments.OrderBy(a => a.PatientName).ToList();
            foreach (var app in sortedAppointments)
            {
                Console.WriteLine(app.ToString());
            }
        }

        private void sortByPatientAge(ICollection<Appointment> appointments)
        {
            var sortedAppointments = appointments.OrderBy(a => a.PatientAge).ToList();
            foreach (var app in sortedAppointments)
            {
                Console.WriteLine(app.ToString());
            }
        }

        private void sortByAppointmentDate(ICollection<Appointment> appointments)
        {
            var sortedAppointments = appointments.OrderBy(a => a.AppointmentDate).ToList();
            foreach (var app in sortedAppointments)
            {
                Console.WriteLine(app.ToString());
            }
        }
        public List<Appointment>? SearchAppointments(SearchModel searchModel)
        {
            try
            {
                var appointments = _appointmentRepository.GetAll();
                appointments = SearchByAppointmentId(appointments, searchModel.Id);
                appointments = SearchByPatientName(appointments, searchModel.PatientName);
                appointments = SearchByPatientAge(appointments, searchModel.PatientAge);
                appointments = SearchByAppointmentDate(appointments, searchModel.AppointmentDate);

                if (appointments != null && appointments.Count > 0)
                {
                    return appointments.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error searching appointments: " + ex.Message);
            }
            return null;
        }

        private ICollection<Appointment> SearchByAppointmentId(ICollection<Appointment> appointments, int appointmentId)
        {
            if (appointmentId > 0)
            {
                appointments = appointments.Where(a => a.Id == appointmentId).ToList();
            }
            return appointments;
        }

        private ICollection<Appointment> SearchByPatientName(ICollection<Appointment> appointments, string patientName)
        {
            if (!string.IsNullOrEmpty(patientName))
            {
                appointments = appointments.Where(a => a.PatientName.Contains(patientName, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            return appointments;
        }

        private ICollection<Appointment> SearchByPatientAge(ICollection<Appointment> appointments, Range<int>? patientAge)
        {
            if (patientAge == null ||  appointments.Count == 0)
            {
                return appointments; 
            }
            return appointments.Where(a => a.PatientAge >= patientAge.MinVal && a.PatientAge <= patientAge.MaxVal).ToList(); ;
            
        }


        private ICollection<Appointment> SearchByAppointmentDate(ICollection<Appointment> appointments, DateTime appointmentDate)
        {
            if (appointmentDate != DateTime.MinValue)
            {
                appointments = appointments.Where(a => a.AppointmentDate.Date == appointmentDate.Date).ToList();
            }
            return appointments;
        }

        public List<Appointment>? SortAppointmentsByDate(SortModel sortmodel)
        {
            throw new NotImplementedException();
        }
    }
}
