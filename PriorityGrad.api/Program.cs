using PriorityGrad.domain.Interfaces;
using PriorityGrad.infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// 1. Servicios
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ITareaRepository, JsonRepository>();

var app = builder.Build();

// 2. Middleware - ¡Aquí faltaban las líneas de Swagger!
app.UseSwagger();           // Genera el archivo JSON de la especificación
app.UseSwaggerUI();         // Habilita la interfaz visual en /swagger

// app.UseHttpsRedirection(); // Comentada temporalmente para evitar el error de puerto
app.MapControllers();

app.Run();