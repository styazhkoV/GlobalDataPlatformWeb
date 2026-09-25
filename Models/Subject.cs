namespace GlobalDataPlatformWeb.Models
{
    public class Subject
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string FullNameLat { get; set; } = string.Empty;
        public string NationalId { get; set; } = string.Empty; // ИИН / ID
        public string CitizenshipCode { get; set; } = "KZA";
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string CivilStatus { get; set; } = string.Empty;


        public PassportData? Passport { get; set; }
        public FinancialData? Finance { get; set; }
        public SecurityData? Security { get; set; }
        public AgroData? Agro { get; set; }
        public List<CurrencyContract> CurrencyContracts { get; set; } = new();
    }
}