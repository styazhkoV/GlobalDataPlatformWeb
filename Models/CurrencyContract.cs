namespace GlobalDataPlatformWeb.Models
{
    public class CurrencyContract
    {
        public int Id { get; set; }
        public string UniqueContractNumber { get; set; } = string.Empty; // УНК
        public string ContractType { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string CurrencyCode { get; set; } = "USD";
        public string ForeignCounterpartyName { get; set; } = string.Empty;
        public string ForeignCounterpartyCountry { get; set; } = string.Empty;
        public string RiskCategory { get; set; } = "Standard";

        public int SubjectId { get; set; }
        public Subject? Subject { get; set; }
    }
}