using GlobalDataPlatformWeb.Models;
using GlobalDataPlatformWeb.ViewModels;

namespace GlobalDataPlatformWeb.Services.Security;

/// <summary>
/// Контракт сервиса применения политик доступа (RBAC) и маскирования чувствительных данных.
/// Гарантирует изоляцию персданных на бэкенде до передачи в слой представления.
/// </summary>
public interface IDataMaskingPolicy
{
    PassportSectionViewModel ProjectPassport(PassportData? passport, string role);
    FinanceSectionViewModel ProjectFinance(FinancialData? finance, List<CurrencyContract> contracts, string role);
    SecuritySectionViewModel ProjectSecurity(SecurityData? security, string role);
    AgroSectionViewModel ProjectAgro(AgroData? agro, string role);
    string MaskNationalId(string? nationalId, string role);
}
