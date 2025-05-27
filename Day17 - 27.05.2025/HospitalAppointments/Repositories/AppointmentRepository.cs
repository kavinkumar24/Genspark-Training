using HospitalAppointments.Models;
using HospitalAppointments.Interfaces;
public class AppointmentRepository : IRepository<int, Appointment>
{
    private readonly List<Appointment> _appointments = new List<Appointment>();
    public Appointment Add(Appointment entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException("Appointment cannot be null");
        }
        if (((HospitalAppointments.Models.Appointment)entity).Id <= 0)
        {
            throw new InvalidOperationException("Appointment ID must be greater than zero.");
        }
        _appointments.Add(entity);
        return entity;
    }
    public Appointment Update(Appointment entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException("Appointment cannot be null");
        }
        if (entity.Id <= 0)
        {
            throw new InvalidOperationException("Appointment ID must be greater than zero.");
        }
        var existingAppointment = GetById(((HospitalAppointments.Models.Appointment)entity).Id);
        if (existingAppointment == null)
        {
            throw new KeyNotFoundException($"Appointment with ID {entity.Id} not found.");
        }
        _appointments.Remove(existingAppointment);
        _appointments.Add(entity);
        return entity;
    }
    public Appointment Delete(int id)
    {
        var appointment = GetById(id);
        if (appointment == null)
        {
            throw new KeyNotFoundException($"Appointment with ID {id} not found.");
        }
        _appointments.Remove(appointment);
        return appointment;
    }

    public Appointment GetById(int id)
    {
        var appointment = _appointments.FirstOrDefault(a => a.Id == id);
        if (appointment == null)
        {
            throw new KeyNotFoundException($"Appointment with ID {id} not found.");
        }
        return appointment;
    }
    public IEnumerable<Appointment> GetAll()
    {
        return _appointments;
    }

}