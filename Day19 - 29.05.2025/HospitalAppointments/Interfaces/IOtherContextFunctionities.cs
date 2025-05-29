using HospitalAppointments.Models;
using HospitalAppointments.Models.DTOs;
namespace HospitalAppointments.Interfaces;

public interface IOtherContextFunctionities
{
    public Task<ICollection<DoctorsBySpecialityResponseDto>> GetDoctorsBySpeciality(string specilaity);
}
