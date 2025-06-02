using FirstAPI.Models;
using FirstAPI.Models.DTOs.DoctorSpecialities;

namespace FirstAPI.Interfaces
{
    public interface IPatientService
    {
        Task<Patient> AddPatient(PatientAddRequestDto patientDto);
        Task<Patient> GetPatientById(int id);
    }
}