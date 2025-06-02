using FirstAPI.Models;
using FirstAPI.Models.DTOs.DoctorSpecialities;

namespace FirstAPI.Interfaces
{
    public interface IAppointmentService
    {
        Task<Appointmnet> AddAppointment(AppointmentAddRequestDto appointmentDto);
        Task<IEnumerable<Appointmnet>> GetAppointmentsByPatientId(int patientId);
        Task<IEnumerable<Appointmnet>> GetAppointmentsByDoctorId(int doctorId);
    }
}