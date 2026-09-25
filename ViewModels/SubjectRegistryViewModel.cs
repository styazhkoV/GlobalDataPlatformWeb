using GlobalDataPlatformWeb.Models.Enums;

namespace GlobalDataPlatformWeb.ViewModels;

public class CurrencyContractViewModel
{
    public string UniqueContractNumber { get; set; } = string.Empty;
    public string ContractType { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public string CurrencyCode { get; set; } = "USD";
    public string ForeignCounterpartyName { get; set; } = string.Empty;
    public string ForeignCounterpartyCountry { get; set; } = string.Empty;
    public string RiskCategory { get; set; } = "Standard";
}

public class PassportSectionViewModel
{
    public AccessLevel AccessLevel { get; set; } = AccessLevel.Restricted;
    public bool IsGranted => AccessLevel == AccessLevel.Granted;
    public bool IsMasked => AccessLevel == AccessLevel.Masked;
    public bool IsRestricted => AccessLevel == AccessLevel.Restricted;

    public string? DisplayPassportNumber { get; set; }
    public string? IssuingAuthority { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string? RegistrationAddress { get; set; }
    public string? MachineReadableZone { get; set; }
    public string? BiometricHash { get; set; }
    public string? AccessNotice { get; set; }
}

public class FinanceSectionViewModel
{
    public AccessLevel AccessLevel { get; set; } = AccessLevel.Restricted;
    public bool IsGranted => AccessLevel == AccessLevel.Granted;
    public bool IsRestricted => AccessLevel == AccessLevel.Restricted;

    public decimal? TotalAnnualIncome { get; set; }
    public decimal? DeclaredAssetsValue { get; set; }
    public string? AmlRiskScore { get; set; }
    public List<CurrencyContractViewModel> Contracts { get; set; } = new();
    public string? AccessNotice { get; set; }
}

public class SecuritySectionViewModel
{
    public AccessLevel AccessLevel { get; set; } = AccessLevel.Restricted;
    public bool IsGranted => AccessLevel == AccessLevel.Granted;
    public bool IsRestricted => AccessLevel == AccessLevel.Restricted;

    public string? ClearanceLevel { get; set; }
    public string? PepStatus { get; set; }
    public string? CriminalRecord { get; set; }
    public bool? IsOnWatchlist { get; set; }
    public List<string> BorderCrossings { get; set; } = new();
    public string? AccessNotice { get; set; }
}

public class AgroSectionViewModel
{
    public AccessLevel AccessLevel { get; set; } = AccessLevel.Restricted;
    public bool IsGranted => AccessLevel == AccessLevel.Granted;
    public bool IsRestricted => AccessLevel == AccessLevel.Restricted;

    public double? TotalLandAreaHectares { get; set; }
    public bool? HasActiveExportQuotas { get; set; }
    public List<string> CadastralPlots { get; set; } = new();
    public Dictionary<string, int> LivestockCount { get; set; } = new();
    public string? AccessNotice { get; set; }
}

public class SubjectCardViewModel
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string FullNameLat { get; set; } = string.Empty;
    public string NationalId { get; set; } = string.Empty;
    public string CitizenshipCode { get; set; } = "KZA";
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; } = string.Empty;
    public string CivilStatus { get; set; } = string.Empty;

    public PassportSectionViewModel Passport { get; set; } = new();
    public FinanceSectionViewModel Finance { get; set; } = new();
    public SecuritySectionViewModel Security { get; set; } = new();
    public AgroSectionViewModel Agro { get; set; } = new();
}

public class RoleOptionViewModel
{
    public string RoleKey { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public bool IsSelected { get; set; }
}

public class SubjectRegistryViewModel
{
    public string CurrentRole { get; set; } = string.Empty;
    public string RoleDisplayName { get; set; } = string.Empty;
    public string RoleDescription { get; set; } = string.Empty;
    public string RoleBadgeClass { get; set; } = string.Empty;
    public string? SearchQuery { get; set; }
    public int TotalSubjectsCount { get; set; }
    public List<SubjectCardViewModel> Subjects { get; set; } = new();
    public List<RoleOptionViewModel> AvailableRoles { get; set; } = new();
}
