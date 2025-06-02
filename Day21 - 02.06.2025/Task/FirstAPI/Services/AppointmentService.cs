using System.Threading.Tasks;
using AutoMapper;
using FirstAPI.Interfaces;
using FirstAPI.Misc;
using FirstAPI.Models;
using FirstAPI.Models.DTOs.DoctorSpecialities;
using FirstAPI.Repositories;


namespace FirstAPI.Services
{
    public class AppointmentService : IAppointmentService
    {
       IRepository<string, Appointmnet> _appointmentRepository;
       
        private readonly IMapper _mapper;
        public AppointmentService(IRepository<string, Appointmnet> appointmentRepository, IMapper mapper)
        {
            _appointmentRepository = appointmentRepository;
            _mapper = mapper;
        }




     public async Task<Appointmnet> AddAppointment(AppointmentAddRequestDto appointment)
        {
            try
            {
                var existingAppointments = await _appointmentRepository.GetAll() ?? new List<Appointmnet>();
                int maxNumber = 0;

                foreach (var appt in existingAppointments)
                {
                    if (appt.AppointmnetNumber != null && appt.AppointmnetNumber.StartsWith("A"))
                    {
                        if (int.TryParse(appt.AppointmnetNumber.Substring(1), out int num))
                        {
                            if (num > maxNumber)
                                maxNumber = num;
                        }
                    }
                }

                string newAppointmentNumber = $"A{maxNumber + 1}";

                var newAppointment = new Appointmnet
                {
                    AppointmnetNumber = newAppointmentNumber,
                    PatientId = appointment.PatientId,
                    DoctorId = appointment.DoctorId,
                    AppointmnetDateTime = appointment.AppointmentDate,
                    Status = "Initiated",
                };

                newAppointment = await _appointmentRepository.Add(newAppointment);
                if (newAppointment == null)
                    throw new Exception("Could not add appointment");

                return newAppointment;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<IEnumerable<Appointmnet>> GetAppointmentsByPatientId(int patientId)
        {
            try
            {
                if (patientId <= 0)
                    throw new ArgumentException("Invalid patient ID");
                if (_appointmentRepository == null)
                    throw new InvalidOperationException("Appointment repository is not initialized.");

                var allAppointments = await _appointmentRepository.GetAll();
                if (allAppointments == null)
                    throw new InvalidOperationException("Appointment repository is empty.");

                return allAppointments.Where(a => a.PatientId == patientId);
            }
            catch (Exception e)
            {
                throw new Exception($"Error retrieving appointments for patient ID {patientId}: {e.Message}");
            }
        }

        public async Task<IEnumerable<Appointmnet>> GetAppointmentsByDoctorId(int doctorId)
        {
            try
            {
                if (doctorId <= 0)
                    throw new ArgumentException("Invalid doctor ID");
                if (_appointmentRepository == null)
                    throw new InvalidOperationException("Appointment repository is not initialized.");

                var allAppointments = await _appointmentRepository.GetAll();
                if (allAppointments == null)
                    throw new InvalidOperationException("Appointment repository is empty.");

                return allAppointments.Where(a => a.DoctorId == doctorId);
            }
            catch (Exception e)
            {
                throw new Exception($"Error retrieving appointments for doctor ID {doctorId}: {e.Message}");
            }
        }
    }
}
