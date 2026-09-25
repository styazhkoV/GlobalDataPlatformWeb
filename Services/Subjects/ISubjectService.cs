using GlobalDataPlatformWeb.ViewModels;

namespace GlobalDataPlatformWeb.Services.Subjects;

public interface ISubjectService
{
    Task<SubjectRegistryViewModel> GetRegistryAsync(string currentRole, string? searchQuery = null);
}
