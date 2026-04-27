using Microsoft.EntityFrameworkCore;
using RickAndMortyMVC.Data;
using RickAndMortyMVC.Services;

var builder = WebApplication.CreateBuilder(args);

// ─── Serveis ───────────────────────────────────────────────────────────────
// Afegir MVC amb vistes
builder.Services.AddControllersWithViews();

// HttpClient per consumir la API de Rick & Morty
builder.Services.AddHttpClient<IRickAndMortyService, RickAndMortyService>();

// Base de dades SQLite (fitxer local, fàcil de desplegar)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")
        ?? "Data Source=rickandmorty.db"));

var app = builder.Build();

// ─── Middleware ────────────────────────────────────────────────────────────
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// ─── Rutes ─────────────────────────────────────────────────────────────────
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Crear la base de dades si no existeix (migracions automàtiques)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.Run();