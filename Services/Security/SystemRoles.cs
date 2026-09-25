namespace GlobalDataPlatformWeb.Services.Security;

/// <summary>
/// Стандартизированный каталог ролей платформы и вспомогательные метаданные для RBAC.
/// </summary>
public static class SystemRoles
{
    public const string SecurityOfficer = "Security_Officer";
    public const string FinancialAuditor = "Financial_Auditor";
    public const string AgroInspector = "Agro_Inspector";
    public const string SystemAdmin = "System_Admin";

    public static readonly IReadOnlyList<string> All = new[]
    {
        SecurityOfficer,
        FinancialAuditor,
        AgroInspector,
        SystemAdmin
    };

    public static string GetDisplayName(string role) => role switch
    {
        SecurityOfficer => "Офицер безопасности (КНБ)",
        FinancialAuditor => "Финансовый аудитор (Минфин)",
        AgroInspector => "Инспектор АПК (Минсельхоз)",
        SystemAdmin => "Администратор системы",
        _ => "Неизвестная роль"
    };

    public static string GetShortDescription(string role) => role switch
    {
        SecurityOfficer => "Допуск к спецдосье, паспортам, биометрии и трансграничным перемещениям.",
        FinancialAuditor => "Допуск к налоговой отчетности, AML-скорингу и валютным контрактам ВЭД.",
        AgroInspector => "Допуск к земельным кадастрам, экспортным квотам и реестру сельхозактивов.",
        SystemAdmin => "Полный сквозной аудит и доступ ко всем государственным реестрам данных.",
        _ => "Гостевой режим"
    };

    public static string GetBadgeClass(string role) => role switch
    {
        SecurityOfficer => "bg-danger",
        FinancialAuditor => "bg-primary",
        AgroInspector => "bg-success",
        SystemAdmin => "bg-dark",
        _ => "bg-secondary"
    };
}
