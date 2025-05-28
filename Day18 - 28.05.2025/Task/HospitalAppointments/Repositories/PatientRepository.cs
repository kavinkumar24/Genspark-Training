using HospitalAppointments.Interfaces;
using HospitalAppointments.Models;
using HospitalAppointments.Contexts;
using Microsoft.EntityFrameworkCore;
namespace HospitalAppointments.Repositories;

public class PatientRepository : Repository<int, Patient>
{
    public PatientRepository(ClinicContext clinicContext) : base(clinicContext)
    {
    }

    public override async Task<Patient> Get(int key)
    {
        var patient = await _clinicContext.patients.SingleOrDefaultAsync(p => p.Id == key);

        return patient ?? throw new Exception("No patient with the given ID");
    }

    public override async Task<IEnumerable<Patient>> GetAll()
    {
        var patients = _clinicContext.patients;
        if (!await patients.AnyAsync())
            throw new Exception("No Patients in the database");
        return await patients.ToListAsync();
    }
}
