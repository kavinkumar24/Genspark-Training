using System;
using HospitalAppointments.Models;
using HospitalAppointments.Models.DTOs;
namespace HospitalAppointments.Interfaces;

public interface IDoctorService
    {
        public Task<Doctor> GetDoctByName(string name);
        public Task<ICollection<DoctorsBySpecialityResponseDto>> GetDoctorsBySpeciality(string speciality);
        public Task<Doctor> AddDoctor(DoctorAddRequestDto doctor);
    }
