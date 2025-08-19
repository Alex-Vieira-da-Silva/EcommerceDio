using Microsoft.AspNetCore.Authentication.JwtBearer;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

// Add the correct namespace for ConsumirEstoque if it exists, for example:
// using ecommerce_microservico.microservico_estoque.Messaging.Consumers;

var builder = WebApplication.CreateBuilder(args);

// 1. Carregar configurações
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("ocelot.json",     optional: false, reloadOnChange: true);

// 2. Registrar serviços de mensageria
builder.Services.AddSingleton<IRabbitMqService, IRabbitMqService>();

// 3. Registrar Auth + Ocelot
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority            = builder.Configuration["Jwt:Issuer"];
        options.Audience             = builder.Configuration["Jwt:Audience"];
        options.RequireHttpsMetadata = false;
    });
builder.Services.AddAuthorization();
builder.Services.AddOcelot(builder.Configuration);

// 4. Registrar MVC/Controllers
builder.Services.AddControllers();

var app = builder.Build();

// 5. Iniciar consumer de estoque ao subir (ideal: mover para IHostedService)
var consumirEstoque = new ConsumirEstoque(builder.Configuration);
consumirEstoque.Start();

// 6. Configurar pipeline HTTP
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// 7. Use Ocelot como middleware final
await app.UseOcelot();

internal interface IRabbitMqService
{
}

// Define ConsumirEstoque class if it does not exist
public class ConsumirEstoque
{
    private readonly IConfiguration _configuration;

    public ConsumirEstoque(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Start()
    {
        // Implement the logic to start consuming messages from RabbitMQ
        // Example:
        // var factory = new ConnectionFactory() { HostName = "localhost" };
        // using var connection = factory.CreateConnection();
        // using var channel = connection.CreateModel();
        // // Setup consumer logic here
    }
}
