using HospitalAppointments.Models;
using HospitalAppointments.Contexts;
using Microsoft.EntityFrameworkCore;
namespace HospitalAppointments.Repositories;

public class DoctorRepository : Repository<int, Doctor>
{
    public DoctorRepository(ClinicContext clinicContext) : base(clinicContext)
    {
    }
    public override async Task<Doctor> Get(int key)
    {
        var doctor = await _clinicContext.doctors
            .SingleOrDefaultAsync(d => d.Id == key);

        return doctor ?? throw new Exception("No doctor with the given ID");
    }
    public override async Task<IEnumerable<Doctor>> GetAll()
    {
        var doctors = _clinicContext.doctors;

        if (!await doctors.AnyAsync())
            throw new Exception("No Doctors in the database");

        return await doctors.ToListAsync();
    }

}
