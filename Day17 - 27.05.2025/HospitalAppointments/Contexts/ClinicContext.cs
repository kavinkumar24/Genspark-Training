using System;
using Microsoft.EntityFrameworkCore;
using HospitalAppointments.Models;
namespace HospitalAppointments.Contexts;

 public class ClinicContext : DbContext
    {
      
        public ClinicContext(DbContextOptions options) :base(options)
        {
            
        }
        public DbSet<Patient> patients { get; set; }
        public DbSet<Doctor> doctors { get; set; }
        public DbSet<Appointment> appointments { get; set; }
        public DbSet<Speciality> specialities { get; set; }
        public DbSet<DoctorSpeciality> doctorSpecialities { get; set; }

    }
       