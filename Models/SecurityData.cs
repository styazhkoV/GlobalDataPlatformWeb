namespace GlobalDataPlatformWeb.Models
{
    public class SecurityData
    {
        public int Id { get; set; }
        public string ClearanceLevel { get; set; } = "Unclassified";
        public string PEPStatus { get; set; } = "No";
        public string CriminalRecord { get; set; } = "None";
        public bool IsOnWatchlist { get; set; }
        public string BorderCrossingsJson { get; set; } = "[]";

        public int SubjectId { get; set; }
        public Subject? Subject { get; set; }
    }
}