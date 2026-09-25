using GlobalDataPlatformWeb.Services.Security;
using GlobalDataPlatformWeb.Services.Subjects;
using Microsoft.AspNetCore.Mvc;

namespace GlobalDataPlatformWeb.Controllers;

public class SubjectsController : Controller
{
    private readonly ISubjectService _subjectService;

    public SubjectsController(ISubjectService subjectService)
    {
        _subjectService = subjectService;
    }

    // GET: /Subjects?currentRole=Security_Officer&search=Иванов
    public async Task<IActionResult> Index(string currentRole = SystemRoles.SecurityOfficer, string? search = null)
    {
        var model = await _subjectService.GetRegistryAsync(currentRole, search);
        return View(model);
    }
}