using GlobalDataPlatformWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace GlobalDataPlatformWeb.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Subject> Subjects => Set<Subject>();
    public DbSet<PassportData> Passports => Set<PassportData>();
    public DbSet<FinancialData> Financials => Set<FinancialData>();
    public DbSet<CurrencyContract> CurrencyContracts => Set<CurrencyContract>();
    public DbSet<SecurityData> Securities => Set<SecurityData>();
    public DbSet<AgroData> Agros => Set<AgroData>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Связь 1:1 — Паспортные данные
        modelBuilder.Entity<Subject>()
            .HasOne(s => s.Passport)
            .WithOne(p => p.Subject)
            .HasForeignKey<PassportData>(p => p.SubjectId)
            .OnDelete(DeleteBehavior.Cascade);

        // Связь 1:1 — Финансы
        modelBuilder.Entity<Subject>()
            .HasOne(s => s.Finance)
            .WithOne(f => f.Subject)
            .HasForeignKey<FinancialData>(f => f.SubjectId)
            .OnDelete(DeleteBehavior.Cascade);

        // Связь 1:1 — Спецслужбы / КНБ
        modelBuilder.Entity<Subject>()
            .HasOne(s => s.Security)
            .WithOne(sec => sec.Subject)
            .HasForeignKey<SecurityData>(sec => sec.SubjectId)
            .OnDelete(DeleteBehavior.Cascade);

        // Связь 1:1 — Агропромышленный комплекс
        modelBuilder.Entity<Subject>()
            .HasOne(s => s.Agro)
            .WithOne(a => a.Subject)
            .HasForeignKey<AgroData>(a => a.SubjectId)
            .OnDelete(DeleteBehavior.Cascade);

        // Связь 1:N — Валютные контракты
        modelBuilder.Entity<Subject>()
            .HasMany(s => s.CurrencyContracts)
            .WithOne(c => c.Subject)
            .HasForeignKey(c => c.SubjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}