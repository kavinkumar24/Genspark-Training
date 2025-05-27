using HospitalAppointments.Models;
namespace HospitalAppointments.Interfaces
{
    public interface IAppointmentService
    {
        void AddAppointment(Appointment appointment);
        void UpdateAppointment(Appointment appointment);
        void DeleteAppointment(int appointmentId);
        Appointment GetAppointmentById(int appointmentId);
        IEnumerable<Appointment> GetAllAppointments();
    }
}