using HospitalAppointments.Models;
using HospitalAppointments.Interfaces;
public class AppointmentService : IAppointmentService
{
    private readonly IRepository<int, Appointment> _appointmentRepository;

    public AppointmentService(IRepository<int, Appointment> repository)
    {
        _appointmentRepository = repository;
    }
    public void AddAppointment(Appointment appointment)
    {
        if (appointment == null)
        {
            throw new ArgumentNullException("Appointment cannot be null");
        }

        _appointmentRepository.Add(appointment);
        if (appointment.Id <= 0)
        {
            throw new InvalidOperationException("Appointment ID must be greater than zero.");
        }
    }

    public void UpdateAppointment(Appointment appointment)
    {
        if (appointment == null)
        {
            throw new ArgumentNullException("Appointment cannot be null");
        }

        if (appointment.Id <= 0)
        {
            throw new InvalidOperationException("Appointment ID must be greater than zero.");
        }
        var existingAppointment = _appointmentRepository.GetById(appointment.Id);
        if (existingAppointment == null)
        {
            throw new KeyNotFoundException($"Appointment with ID {appointment.Id} not found.");
        }
        _appointmentRepository.Update(appointment);
    }
    public void DeleteAppointment(int appointmentId)
    {
        var appointment = _appointmentRepository.GetById(appointmentId);
        if (appointment == null)
        {
            throw new KeyNotFoundException($"Appointment with ID {appointmentId} not found.");
        }
        if (appointmentId <= 0)
        {
            throw new InvalidOperationException("Appointment ID must be greater than zero.");
        }
        if (appointment.AppointmentDateTime < DateTime.Now)
        {
            throw new InvalidOperationException("Cannot delete past appointments.");
        }
        _appointmentRepository.Delete(appointmentId);
    }
    public Appointment GetAppointmentById(int appointmentId)
    {
        var appointment = _appointmentRepository.GetById(appointmentId);
        if (appointment == null)
        {
            throw new KeyNotFoundException($"Appointment with ID {appointmentId} not found.");
        }

        return appointment;
    }
    public IEnumerable<Appointment> GetAllAppointments()
    {
        var appointments = _appointmentRepository.GetAll();
        if (appointments == null || !appointments.Any())
        {
            throw new InvalidOperationException("No appointments found.");
        }

        return appointments;
    }
}