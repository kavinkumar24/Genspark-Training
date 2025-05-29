using HospitalAppointments.Models;
using HospitalAppointments.Models.DTOs;
using HospitalAppointments.Interfaces;
using HospitalAppointments.Contexts;
namespace HosipitalAppointments.Misc
{
public class OtherFuncinalitiesImplementation : IOtherContextFunctionities
    {
        private readonly ClinicContext _clinicContext;

        public OtherFuncinalitiesImplementation(ClinicContext clinicContext)
        {
            _clinicContext = clinicContext;
        }

        public async Task<ICollection<DoctorsBySpecialityResponseDto>> GetDoctorsBySpeciality(string specilaity)
        {
            var result = await _clinicContext.GetDoctorsBySpeciality(specilaity);
            return result;
        }
    }
}