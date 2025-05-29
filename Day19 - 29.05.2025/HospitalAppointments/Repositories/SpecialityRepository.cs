using System;
using HospitalAppointments.Models;
using HospitalAppointments.Contexts;
using Microsoft.EntityFrameworkCore;
namespace HospitalAppointments.Repositories;

public class SpecialityRepository : Repository<int, Speciality>
{
    public SpecialityRepository(ClinicContext clinicContext) : base(clinicContext)
    {
    }

    public override async Task<Speciality> Get(int key)
    {
        var speciality = await _clinicContext.specialities
            .SingleOrDefaultAsync(s => s.Id == key);

        return speciality ?? throw new Exception("No Speciality with the given ID");
    }

    public override async Task<IEnumerable<Speciality>> GetAll()
    {
        var specialities = _clinicContext.specialities;

        if (!await specialities.AnyAsync())
            throw new Exception("No Specialities in the database");

        return await specialities.ToListAsync();
    }

}
