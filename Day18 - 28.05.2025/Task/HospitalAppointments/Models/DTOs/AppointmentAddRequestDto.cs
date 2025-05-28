using System;

namespace HospitalAppointments.Models.DTOs;

public class AppointmentAddRequestDto
{
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public DateTime AppointmentDateTime { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Description { get; set; }

}
