using Amazon.SecretsManager;
using Challenge.Devsu.Api.Middlewares;
using Challenge.Devsu.Infrastructure.Configurations;
using Challenge.Devsu.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Logging para Kubernetes/Argo
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(static s =>
{
    s.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1.0.0",
        Title = "Api banco",
        Description = "Este microservicio se encarga de guardar datos de clientes, cuentas y visualizar movimientos"
    });
    var fileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var filePath = Path.Combine(AppContext.BaseDirectory, fileName);
    s.IncludeXmlComments(filePath);
});

builder.Services
    .AddControllers(o =>
    {
        o.Filters.Add<Challenge.Devsu.Api.Filters.ValidationFilter>();
    });

builder.Services.AddDbContext<DbDataContext>((serviceProvider, options) =>
{
    var dbConfigService = serviceProvider.GetRequiredService<DatabaseConfigService>();
    var connectionString = dbConfigService.GetConnectionString();
    options.UseNpgsql(connectionString);
});
builder.Services.RegisterDependencies();
var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

//Configurar Swagger
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
await app.RunAsync();
