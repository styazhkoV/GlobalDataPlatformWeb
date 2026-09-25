namespace GlobalDataPlatformWeb.Models.Enums;

/// <summary>
/// Уровень доступа к определенному домену данных субъекта согласно матрице RBAC.
/// </summary>
public enum AccessLevel
{
    /// <summary>
    /// Полный доступ к исходным данным домена (без маскирования).
    /// </summary>
    Granted = 1,

    /// <summary>
    /// Ограниченный доступ: чувствительные реквизиты маскируются на уровне DTO.
    /// </summary>
    Masked = 2,

    /// <summary>
    /// Доступ заблокирован: данные не загружаются и не передаются в DTO (Zero Trust).
    /// </summary>
    Restricted = 3
}
