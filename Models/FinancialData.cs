namespace GlobalDataPlatformWeb.Models
{
    public class FinancialData
    {
        public int Id { get; set; }
        public decimal TotalAnnualIncome { get; set; }
        public decimal DeclaredAssetsValue { get; set; }
        public string AMLRiskScore { get; set; } = "Low";

        public int SubjectId { get; set; }
        public Subject? Subject { get; set; }
    }
}