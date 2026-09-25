using Bogus;
using GlobalDataPlatformWeb.Models;

namespace GlobalDataPlatformWeb.Data;

public static class FullDataSeeder
{
    public static List<Subject> GenerateFullData(int count = 10)
    {
        var subjectFaker = new Faker<Subject>("ru")
            .RuleFor(s => s.FullName, f => f.Name.FullName())
            .RuleFor(s => s.FullNameLat, (f, s) => f.Name.FirstName() + " " + f.Name.LastName())
            .RuleFor(s => s.NationalId, f => f.Random.ReplaceNumbers("############"))
            .RuleFor(s => s.CitizenshipCode, f => f.PickRandom("KZA", "KGZ", "UZB", "RUS"))
            .RuleFor(s => s.DateOfBirth, f => f.Date.Past(50, DateTime.Now.AddYears(-18)))
            .RuleFor(s => s.Gender, f => f.PickRandom("Мужской", "Женский"))
            .RuleFor(s => s.CivilStatus, f => f.PickRandom("Женат/Замужем", "Холост/Не замужем", "В разводе"));

        var subjects = subjectFaker.Generate(count);

        var random = new Random();

        foreach (var subject in subjects)
        {
            // 1. Паспортные данные
            subject.Passport = new Faker<PassportData>("ru")
                .RuleFor(p => p.PassportNumber, f => "N" + f.Random.ReplaceNumbers("########"))
                .RuleFor(p => p.IssuingAuthority, f => "МВД РК / МВД РФ")
                .RuleFor(p => p.ExpiryDate, f => f.Date.Future(10))
                .RuleFor(p => p.RegistrationAddress, f => f.Address.FullAddress())
                .RuleFor(p => p.MachineReadableZone, f => $"P<KZA{subject.FullNameLat.Replace(" ", "<")}<<<<<<<<<<<<<<<<")
                .RuleFor(p => p.BiometricHash, f => Guid.NewGuid().ToString("N"))
                .Generate();

            // 2. Финансы
            subject.Finance = new Faker<FinancialData>()
                .RuleFor(f => f.TotalAnnualIncome, f => f.Finance.Amount(5000, 500000))
                .RuleFor(f => f.DeclaredAssetsValue, f => f.Finance.Amount(10000, 2000000))
                .RuleFor(f => f.AMLRiskScore, f => f.PickRandom("Low", "Medium", "High", "Critical"))
                .Generate();

            // 3. Спецслужбы
            subject.Security = new Faker<SecurityData>()
                .RuleFor(s => s.ClearanceLevel, f => f.PickRandom("Unclassified", "Confidential", "Secret", "Top Secret"))
                .RuleFor(s => s.PEPStatus, f => f.PickRandom("No", "Yes (Direct)", "Yes (Affiliated)"))
                .RuleFor(s => s.CriminalRecord, f => f.PickRandom("None", "Expunged", "Active Investigation"))
                .RuleFor(s => s.IsOnWatchlist, f => f.Random.Bool(0.2f))
                .RuleFor(s => s.BorderCrossingsJson, f => "[\"2025-05-12 ALA->DXB\", \"2025-11-03 NQZ->IST\"]")
                .Generate();

            // 4. Агропромышленный комплекс
            subject.Agro = new Faker<AgroData>()
                .RuleFor(a => a.TotalLandAreaHectares, f => Math.Round(f.Random.Double(10, 1500), 2))
                .RuleFor(a => a.HasActiveExportQuotas, f => f.Random.Bool())
                .RuleFor(a => a.CadastralPlotsJson, f => "[\"05-084-012-001\", \"05-084-012-002\"]")
                .RuleFor(a => a.LivestockCountJson, f => "{\"КРС\": 120, \"МРС\": 450}")
                .Generate();

            // 5. Валютные контракты (1:N)
            int contractCount = random.Next(1, 4);
            var contractFaker = new Faker<CurrencyContract>()
                .RuleFor(c => c.UniqueContractNumber, f => "UNC-" + f.Random.ReplaceNumbers("##########"))
                .RuleFor(c => c.ContractType, f => f.PickRandom("Импорт оборудования", "Экспорт сельхозпродукции", "Услуги IT"))
                .RuleFor(c => c.TotalAmount, f => f.Finance.Amount(10000, 1000000))
                .RuleFor(c => c.CurrencyCode, f => f.PickRandom("USD", "EUR", "CNY", "RUB"))
                .RuleFor(c => c.ForeignCounterpartyName, f => f.Company.CompanyName())
                .RuleFor(c => c.ForeignCounterpartyCountry, f => f.Address.CountryCode())
                .RuleFor(c => c.RiskCategory, f => f.PickRandom("Standard", "Watchlist", "High Risk"));

            subject.CurrencyContracts = contractFaker.Generate(contractCount);
        }

        return subjects;
    }
}