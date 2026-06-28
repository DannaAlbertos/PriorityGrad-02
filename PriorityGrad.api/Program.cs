using PriorityGrad.domain.Interfaces;
using PriorityGrad.infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CONFIGURACIÓN DE RUTA: 
// Pasamos explícitamente la ruta de la carpeta Data de tu proyecto web
string pathWeb = @"C:\Users\danna\source\repos\PriorityGrad\PriorityGrad.web\Data";

//builder.Services.AddScoped<ITareaRepository>(sp => new JsonRepository(pathWeb));
// Cambia la línea 14 de tu Program.cs en el proyecto API por esta:
builder.Services.AddScoped<ITareaRepository, JsonRepository>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();