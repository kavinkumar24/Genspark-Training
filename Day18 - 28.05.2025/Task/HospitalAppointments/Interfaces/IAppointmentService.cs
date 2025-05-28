using HospitalAppointments.Models;
namespace HospitalAppointments.Interfaces
{
    public interface IAppointmentService
    {
        public Task<Appointment> AddAppointment(Appointment appointment);
        public Task<Appointment> GetAppointmentById(int id);
        public Task<IEnumerable<Appointment>> GetAllAppointments();

        public Task<Appointment> UpdateAppointment(int id, Appointment appointment);
        public Task<Appointment> DeleteAppointment(int id);
    }
}