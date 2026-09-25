namespace GlobalDataPlatformWeb.Models
{
    public class PassportData
    {
        public int Id { get; set; }
        public string PassportNumber { get; set; } = string.Empty;
        public string IssuingAuthority { get; set; } = string.Empty;
        public DateTime ExpiryDate { get; set; }
        public string RegistrationAddress { get; set; } = string.Empty;
        public string MachineReadableZone { get; set; } = string.Empty;
        public string BiometricHash { get; set; } = string.Empty;

        // Внешний ключ для связи 1:1 с Subject
        public int SubjectId { get; set; }
        public Subject? Subject { get; set; }
    }
}