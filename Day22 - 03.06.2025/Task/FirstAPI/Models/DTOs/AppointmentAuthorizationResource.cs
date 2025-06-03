namespace FirstAPI.Models.DTOs.DoctorSpecialities
{
    public class AppointmentAuthorizationResource
    {
        public string AppointmentId { get; set; } = string.Empty;
        public AppointmentUpdateRequestDto AppointmentUpdateRequest { get; set; }
    }

}