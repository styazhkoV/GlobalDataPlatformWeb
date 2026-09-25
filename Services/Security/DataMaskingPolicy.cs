using System.Text.Json;
using GlobalDataPlatformWeb.Models;
using GlobalDataPlatformWeb.Models.Enums;
using GlobalDataPlatformWeb.ViewModels;

namespace GlobalDataPlatformWeb.Services.Security;

public class DataMaskingPolicy : IDataMaskingPolicy
{
    public PassportSectionViewModel ProjectPassport(PassportData? passport, string role)
    {
        var vm = new PassportSectionViewModel();

        if (passport == null)
        {
            vm.AccessLevel = AccessLevel.Restricted;
            vm.AccessNotice = "Паспортные данные не зарегистрированы в системе.";
            return vm;
        }

        bool hasFullAccess = role is SystemRoles.SecurityOfficer or SystemRoles.SystemAdmin;
        bool hasLimitedAccess = role is SystemRoles.FinancialAuditor or SystemRoles.AgroInspector;

        if (hasFullAccess)
        {
            vm.AccessLevel = AccessLevel.Granted;
            vm.DisplayPassportNumber = passport.PassportNumber;
            vm.IssuingAuthority = passport.IssuingAuthority;
            vm.ExpiryDate = passport.ExpiryDate;
            vm.RegistrationAddress = passport.RegistrationAddress;
            vm.MachineReadableZone = passport.MachineReadableZone;
            vm.BiometricHash = passport.BiometricHash;
            vm.AccessNotice = "Полный допуск: биометрия и машиночитаемая зона верифицированы.";
        }
        else if (hasLimitedAccess)
        {
            vm.AccessLevel = AccessLevel.Masked;
            vm.DisplayPassportNumber = MaskNumber(passport.PassportNumber, visibleTrailingChars: 4);
            vm.IssuingAuthority = passport.IssuingAuthority;
            vm.ExpiryDate = passport.ExpiryDate;
            vm.RegistrationAddress = passport.RegistrationAddress;
            // Биометрия и MRZ не передаются в DTO (Zero Trust)
            vm.MachineReadableZone = null;
            vm.BiometricHash = null;
            vm.AccessNotice = "Маскирование ПДн: Полный номер и биометрия доступны только Офицерам КНБ.";
        }
        else
        {
            vm.AccessLevel = AccessLevel.Restricted;
            vm.AccessNotice = "Доступ к паспортному досье ограничен регламентом информационной безопасности.";
        }

        return vm;
    }

    public FinanceSectionViewModel ProjectFinance(FinancialData? finance, List<CurrencyContract> contracts, string role)
    {
        var vm = new FinanceSectionViewModel();

        bool hasAccess = role is SystemRoles.FinancialAuditor or SystemRoles.SecurityOfficer or SystemRoles.SystemAdmin;

        if (!hasAccess || finance == null)
        {
            vm.AccessLevel = AccessLevel.Restricted;
            vm.AccessNotice = $"Финансовые данные и ВЭД-контракты закрыты для роли '{SystemRoles.GetDisplayName(role)}'. Требуется допуск Минфина или КНБ.";
            return vm;
        }

        vm.AccessLevel = AccessLevel.Granted;
        vm.TotalAnnualIncome = finance.TotalAnnualIncome;
        vm.DeclaredAssetsValue = finance.DeclaredAssetsValue;
        vm.AmlRiskScore = finance.AMLRiskScore;
        vm.AccessNotice = "Доступ предоставлен: финансовый скоринг и реестр ВЭД актуальны.";

        vm.Contracts = contracts.Select(c => new CurrencyContractViewModel
        {
            UniqueContractNumber = c.UniqueContractNumber,
            ContractType = c.ContractType,
            TotalAmount = c.TotalAmount,
            CurrencyCode = c.CurrencyCode,
            ForeignCounterpartyName = c.ForeignCounterpartyName,
            ForeignCounterpartyCountry = c.ForeignCounterpartyCountry,
            RiskCategory = c.RiskCategory
        }).ToList();

        return vm;
    }

    public SecuritySectionViewModel ProjectSecurity(SecurityData? security, string role)
    {
        var vm = new SecuritySectionViewModel();

        bool hasAccess = role is SystemRoles.SecurityOfficer or SystemRoles.SystemAdmin;

        if (!hasAccess || security == null)
        {
            vm.AccessLevel = AccessLevel.Restricted;
            vm.AccessNotice = "Служебная тайна (Гриф 'Совершенно секретно'). Сведения спецслужб доступны только Офицерам безопасности.";
            return vm;
        }

        vm.AccessLevel = AccessLevel.Granted;
        vm.ClearanceLevel = security.ClearanceLevel;
        vm.PepStatus = security.PEPStatus;
        vm.CriminalRecord = security.CriminalRecord;
        vm.IsOnWatchlist = security.IsOnWatchlist;
        vm.AccessNotice = "Служебный допуск подтвержден. Контроль по линии спецведомств.";

        if (!string.IsNullOrWhiteSpace(security.BorderCrossingsJson))
        {
            try
            {
                var list = JsonSerializer.Deserialize<List<string>>(security.BorderCrossingsJson);
                if (list != null)
                {
                    vm.BorderCrossings = list;
                }
            }
            catch
            {
                vm.BorderCrossings = new List<string> { security.BorderCrossingsJson };
            }
        }

        return vm;
    }

    public AgroSectionViewModel ProjectAgro(AgroData? agro, string role)
    {
        var vm = new AgroSectionViewModel();

        bool hasAccess = role is SystemRoles.AgroInspector or SystemRoles.SecurityOfficer or SystemRoles.SystemAdmin;

        if (!hasAccess || agro == null)
        {
            vm.AccessLevel = AccessLevel.Restricted;
            vm.AccessNotice = $"Реестр АПК недоступен для роли '{SystemRoles.GetDisplayName(role)}'. Требуется допуск Инспектора Минсельхоза.";
            return vm;
        }

        vm.AccessLevel = AccessLevel.Granted;
        vm.TotalLandAreaHectares = agro.TotalLandAreaHectares;
        vm.HasActiveExportQuotas = agro.HasActiveExportQuotas;
        vm.AccessNotice = "Доступ предоставлен: кадастровый учет и квоты подтверждены.";

        if (!string.IsNullOrWhiteSpace(agro.CadastralPlotsJson))
        {
            try
            {
                var plots = JsonSerializer.Deserialize<List<string>>(agro.CadastralPlotsJson);
                if (plots != null) vm.CadastralPlots = plots;
            }
            catch
            {
                vm.CadastralPlots = new List<string> { agro.CadastralPlotsJson };
            }
        }

        if (!string.IsNullOrWhiteSpace(agro.LivestockCountJson))
        {
            try
            {
                var dict = JsonSerializer.Deserialize<Dictionary<string, int>>(agro.LivestockCountJson);
                if (dict != null) vm.LivestockCount = dict;
            }
            catch
            {
                vm.LivestockCount = new Dictionary<string, int>();
            }
        }

        return vm;
    }

    public string MaskNationalId(string? nationalId, string role)
    {
        if (string.IsNullOrWhiteSpace(nationalId)) return "—";

        // Для офицеров КНБ и Админа полный ИИН, для остальных оставляем первые 4 и последние 2 цифры
        if (role is SystemRoles.SecurityOfficer or SystemRoles.SystemAdmin)
        {
            return nationalId;
        }

        if (nationalId.Length <= 6) return new string('*', nationalId.Length);

        return $"{nationalId[..4]}******{nationalId[^2..]}";
    }

    private static string MaskNumber(string? value, int visibleTrailingChars)
    {
        if (string.IsNullOrWhiteSpace(value)) return "—";
        if (value.Length <= visibleTrailingChars) return new string('*', value.Length);

        string maskedPrefix = new('*', Math.Max(0, value.Length - visibleTrailingChars));
        string trailing = value[^visibleTrailingChars..];
        return $"{maskedPrefix}{trailing}";
    }
}
