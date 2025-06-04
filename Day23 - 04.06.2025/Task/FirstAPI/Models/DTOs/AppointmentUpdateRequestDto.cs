namespace FirstAPI.Models.DTOs.DoctorSpecialities
{
    public class AppointmentUpdateRequestDto
    {
        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}