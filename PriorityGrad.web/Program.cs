using PriorityGrad.domain.Interfaces;
using PriorityGrad.infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Registra los servicios de controladores y vistas
builder.Services.AddControllersWithViews();

// CONFIGURACIÓN CLAVE (Inyección de dependencias)
// Conecta la Interfaz (Domain) con la Clase concreta (Infrastructure)
builder.Services.AddScoped<ITareaRepository, JsonRepository>();

var app = builder.Build();

// Pipeline de configuración
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Importante para tus archivos CSS/JS
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();