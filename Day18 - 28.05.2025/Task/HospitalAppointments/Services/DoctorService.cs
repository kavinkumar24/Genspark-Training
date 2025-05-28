using System;
using HospitalAppointments.Interfaces;
namespace HospitalAppointments.Services;

using HospitalAppointments.Models;
using HospitalAppointments.Models.DTOs;

public class DoctorService : IDoctorService
{
    private readonly IRepository<int, Doctor> _doctorRepository;
    private readonly IRepository<int, Speciality> _specialityRepository;
    private readonly IRepository<int, DoctorSpeciality> _doctorSpecialityRepository;
    public DoctorService(IRepository<int, Doctor> doctorRepository, IRepository<int, Speciality> specialityRepository, IRepository<int, DoctorSpeciality> doctorSpecialityRepository)
    {
        _doctorRepository = doctorRepository;
        _specialityRepository = specialityRepository;
        _doctorSpecialityRepository = doctorSpecialityRepository;

    }


    public async Task<Doctor> GetDoctByName(string name)
    {
        var allDoctors = await _doctorRepository.GetAll();
        var doctor = allDoctors.FirstOrDefault(d => d.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        if (doctor == null)
        {
            throw new Exception($"Doctor with name {name} not found.");
        }
        if (doctor.Status != "Active")
        {
            throw new Exception($"Doctor {name} is not active.");
        }

        return doctor;
    }

    public async Task<ICollection<Doctor>> GetDoctorsBySpeciality(string speciality)
    {
        var allSpecialities = await _specialityRepository.GetAll();
        var specialityEntity = allSpecialities.FirstOrDefault(s => s.Name.Equals(speciality, StringComparison.OrdinalIgnoreCase));
        if (specialityEntity == null)
        {
            throw new Exception($"Speciality {speciality} not found.");
        }
        if (specialityEntity.Status != "Active")
        {
            throw new Exception($"Speciality {speciality} is not active.");
        }
        if (specialityEntity.DoctorSpecialities == null || !specialityEntity.DoctorSpecialities.Any())
        {
            throw new Exception($"No doctors found for speciality {speciality}.");
        }

        var doctorSpecialities = await _doctorSpecialityRepository.GetAll();
        var doctorIds = doctorSpecialities
            .Where(d => d.SpecialityId == specialityEntity.Id)
            .Select(d => d.DoctorId);

        var doctors = await _doctorRepository.GetAll();
        return doctors.Where(d => doctorIds.Contains(d.Id)).ToList();
    }

    public async Task<Doctor> AddDoctor(DoctorAddRequestDto doctorDto)
    {
        var doctor = new Doctor
        {
            Name = doctorDto.Name,
            YearsOfExperience = doctorDto.YearsOfExperience,
            Status = "Active"

        };
        if (string.IsNullOrWhiteSpace(doctor.Name))
        {
            throw new Exception("Doctor name cannot be empty or whitespace.");
        }
        if (doctor.YearsOfExperience < 0)
        {
            throw new Exception("Doctor years of experience cannot be negative.");
        }

        await _doctorRepository.Add(doctor);

        var addedDoctor = await _doctorRepository.Get(doctor.Id);
        if (addedDoctor == null)
        {
            throw new Exception("Error adding doctor.");
        }
        if (doctorDto.Specialities == null || !doctorDto.Specialities.Any())
        {
            throw new Exception("Doctor must have at least one speciality.");
        }
        if (doctorDto.Specialities.Any(s => string.IsNullOrWhiteSpace(s.Name)))
        {
            throw new Exception("Doctor specialities cannot contain empty or whitespace values.");
        }
        foreach (var specialityDto in doctorDto.Specialities)
        {
            var specialities = await _specialityRepository.GetAll();
            var specialityEntity = specialities.FirstOrDefault(s =>
                s.Name.Equals(specialityDto.Name, StringComparison.OrdinalIgnoreCase));


            if (specialityEntity == null)
            {
                throw new Exception($"Speciality {specialityDto.Name} not found.");
            }


            var doctorSpeciality = new DoctorSpeciality
            {
                DoctorId = addedDoctor.Id,
                SpecialityId = specialityEntity.Id
            };


            await _doctorSpecialityRepository.Add(doctorSpeciality);
        }


        return addedDoctor;
    }
    
    
   
}
