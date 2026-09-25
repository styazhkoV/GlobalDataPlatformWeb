using GlobalDataPlatformWeb.Services.Security;
using Microsoft.AspNetCore.Mvc;

namespace GlobalDataPlatformWeb.Controllers;

public class HomeController : Controller
{
    // Демо-пароли для ролей (константы из SystemRoles)
    private static readonly Dictionary<string, string> RolePasswords = new()
    {
        { SystemRoles.SecurityOfficer, "knb2026" },
        { SystemRoles.FinancialAuditor, "fin2026" },
        { SystemRoles.AgroInspector, "agro2026" },
        { SystemRoles.SystemAdmin, "admin123" }
    };

    public IActionResult Index(string? role, bool isAuthorized = false)
    {
        ViewBag.IsAuthorized = isAuthorized;
        ViewBag.CurrentRole = role ?? SystemRoles.SecurityOfficer;
        return View();
    }

    [HttpPost]
    public IActionResult Login(string selectedRole, string password)
    {
        // Проверка пароля роли
        if (RolePasswords.TryGetValue(selectedRole, out var correctPassword) && password == correctPassword)
        {
            // Переход к матрице субъектов при успешной авторизации
            return RedirectToAction("Index", "Subjects", new { currentRole = selectedRole });
        }

        TempData["ErrorMessage"] = "Неверный пароль для выбранной роли! Обратитесь к подсказке в форме входа.";
        return RedirectToAction("Index");
    }
}