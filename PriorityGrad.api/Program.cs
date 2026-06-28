using PriorityGrad.domain.Interfaces;
using PriorityGrad.infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Recomendado para probar tus endpoints

// Registro de dependencia
builder.Services.AddScoped<ITareaRepository, JsonRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();