using ProductApi.Repositories.Implementations;
using ProductApi.Repositories.Interfaces;
using ProductApi.Services.Implementations;
using ProductApi.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Inyección de dependencias (Etapa 9)
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();