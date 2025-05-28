using System;
using HospitalAppointments.Models;
namespace HospitalAppointments.Interfaces;

public interface IPatientService
{
    public Task<Patient> GetPatientById(int id);
    public Task<Patient> AddPatient(Patient patient);
    public Task<Patient> UpdatePatient(Patient patient);
    public Task<Patient> DeletePatient(int id);
    public Task<IEnumerable<Patient>> GetAllPatients();

}
