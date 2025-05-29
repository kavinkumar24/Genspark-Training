using HospitalAppointments.Models;
using HospitalAppointments.Contexts;
using Microsoft.EntityFrameworkCore;
namespace HospitalAppointments.Repositories;

public class AppointmentRepository : Repository<string, Appointment>
{
    public AppointmentRepository(ClinicContext clinicContext) : base(clinicContext)
    {
    }

    public override async Task<Appointment> Get(string key)
    {
        var appointment = await _clinicContext.appointments
            .SingleOrDefaultAsync(a => a.AppointmentNumber == key);

        return appointment ?? throw new Exception("No appointment with the given ID");
    }

    public override async Task<IEnumerable<Appointment>> GetAll()
    {
        var appointments = _clinicContext.appointments;
        
        if (!await appointments.AnyAsync())
            throw new Exception("No Appointments in the database");
        
        return await appointments.ToListAsync();
    }
}