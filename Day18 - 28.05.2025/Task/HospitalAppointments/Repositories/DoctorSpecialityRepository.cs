using HospitalAppointments.Models;
using HospitalAppointments.Contexts;
using Microsoft.EntityFrameworkCore;
namespace HospitalAppointments.Repositories;


public class DoctorSpecialityRepository : Repository<int, DoctorSpeciality>
{
    public DoctorSpecialityRepository(ClinicContext clinicContext) : base(clinicContext)
    {
    }
    public override async Task<DoctorSpeciality> Get(int key)
    {
        var doctorSpeciality = await _clinicContext.doctorSpecialities
            .SingleOrDefaultAsync(ds => ds.SerialNumber == key);

        return doctorSpeciality ?? throw new Exception("No DoctorSpeciality with the given Serial Number");
    }

    public override async Task<IEnumerable<DoctorSpeciality>> GetAll()
    {
        var doctorSpecialities = _clinicContext.doctorSpecialities;


        if (!await doctorSpecialities.AnyAsync())
            throw new Exception("No DoctorSpecialities in the database");

        return await doctorSpecialities.ToListAsync();
    }
    

}
