using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HospitalAppointments.Models
{
    public class Appointment
    {
        
        public int Id { get; set; }
        public string AppointmentNumber { get; set; } = string.Empty;
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Doctor? Doctor { get; set; }
       
        public Patient? Patient { get; set; }
    }
}