namespace FirstApiApp.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Diagnosis? Diagnosis { get; set; }

    }

    public class Diagnosis
    {
        public string? Disease { get; set; }
        public string? Treatment { get; set; }
        public DateTime? DiagnosisDate { get; set; }
    }

}