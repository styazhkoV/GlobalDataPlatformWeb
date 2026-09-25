using GlobalDataPlatformWeb.Data;
using GlobalDataPlatformWeb.Services.Security;
using GlobalDataPlatformWeb.Services.Subjects;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Подключаем сервисы MVC
builder.Services.AddControllersWithViews();

// 2. Настраиваем SQLite базу данных
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=global_platform.db"));

// 3. Сервисы предметной области и политики RBAC (Zero Trust Architecture)
builder.Services.AddScoped<IDataMaskingPolicy, DataMaskingPolicy>();
builder.Services.AddScoped<ISubjectService, SubjectService>();

var app = builder.Build();

// 4. Автоматическая инициализация БД и заполнение тестовыми данными
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated(); // Создаем базу, если ее еще нет

    if (!db.Subjects.Any())
    {
        var fakeSubjects = FullDataSeeder.GenerateFullData(15);
        db.Subjects.AddRange(fakeSubjects);
        db.SaveChanges();
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

// 5. Маршрутизация по умолчанию (Главная страница-портфолио)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();