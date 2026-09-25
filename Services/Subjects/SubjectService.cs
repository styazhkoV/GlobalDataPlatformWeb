using GlobalDataPlatformWeb.Data;
using GlobalDataPlatformWeb.Services.Security;
using GlobalDataPlatformWeb.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace GlobalDataPlatformWeb.Services.Subjects;

public class SubjectService : ISubjectService
{
    private readonly AppDbContext _context;
    private readonly IDataMaskingPolicy _maskingPolicy;

    public SubjectService(AppDbContext context, IDataMaskingPolicy maskingPolicy)
    {
        _context = context;
        _maskingPolicy = maskingPolicy;
    }

    public async Task<SubjectRegistryViewModel> GetRegistryAsync(string currentRole, string? searchQuery = null)
    {
        // Нормализация роли по умолчанию
        if (!SystemRoles.All.Contains(currentRole))
        {
            currentRole = SystemRoles.SecurityOfficer;
        }

        var query = _context.Subjects
            .AsNoTracking()
            .Include(s => s.Passport)
            .Include(s => s.Finance)
            .Include(s => s.Security)
            .Include(s => s.Agro)
            .Include(s => s.CurrencyContracts)
            .AsQueryable();

        // Поиск по ФИО или ИИН (если задан)
        if (!string.IsNullOrWhiteSpace(searchQuery))
        {
            var trimmedSearch = searchQuery.Trim().ToLower();
            query = query.Where(s =>
                s.FullName.ToLower().Contains(trimmedSearch) ||
                s.FullNameLat.ToLower().Contains(trimmedSearch) ||
                s.NationalId.Contains(trimmedSearch));
        }

        var subjects = await query.ToListAsync();

        var cardViewModels = subjects.Select(s => new SubjectCardViewModel
        {
            Id = s.Id,
            FullName = s.FullName,
            FullNameLat = s.FullNameLat,
            NationalId = _maskingPolicy.MaskNationalId(s.NationalId, currentRole),
            CitizenshipCode = s.CitizenshipCode,
            DateOfBirth = s.DateOfBirth,
            Gender = s.Gender,
            CivilStatus = s.CivilStatus,
            Passport = _maskingPolicy.ProjectPassport(s.Passport, currentRole),
            Finance = _maskingPolicy.ProjectFinance(s.Finance, s.CurrencyContracts, currentRole),
            Security = _maskingPolicy.ProjectSecurity(s.Security, currentRole),
            Agro = _maskingPolicy.ProjectAgro(s.Agro, currentRole)
        }).ToList();

        var availableRoles = SystemRoles.All.Select(r => new RoleOptionViewModel
        {
            RoleKey = r,
            DisplayName = SystemRoles.GetDisplayName(r),
            IsSelected = r == currentRole
        }).ToList();

        return new SubjectRegistryViewModel
        {
            CurrentRole = currentRole,
            RoleDisplayName = SystemRoles.GetDisplayName(currentRole),
            RoleDescription = SystemRoles.GetShortDescription(currentRole),
            RoleBadgeClass = SystemRoles.GetBadgeClass(currentRole),
            SearchQuery = searchQuery,
            TotalSubjectsCount = cardViewModels.Count,
            Subjects = cardViewModels,
            AvailableRoles = availableRoles
        };
    }
}
