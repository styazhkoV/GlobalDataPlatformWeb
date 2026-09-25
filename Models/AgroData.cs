namespace GlobalDataPlatformWeb.Models
{
    public class AgroData
    {
        public int Id { get; set; }
        public double TotalLandAreaHectares { get; set; }
        public bool HasActiveExportQuotas { get; set; }
        public string CadastralPlotsJson { get; set; } = "[]";
        public string LivestockCountJson { get; set; } = "{}";

        public int SubjectId { get; set; }
        public Subject? Subject { get; set; }
    }
}