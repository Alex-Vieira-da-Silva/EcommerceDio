using Microsoft.AspNetCore.Authentication.JwtBearer;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// 1. Carregar configurações
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("ocelot.json",     optional: false, reloadOnChange: true);

// 2. Registrar serviços de mensageria
builder.Services.AddSingleton<IRabbitMqService, RabbitMqService>();

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


// Interface de serviço RabbitMQ
internal interface IRabbitMqService
{
    void EnviarMensagem(string mensagem);
}


// Implementação concreta do serviço RabbitMQ
public class RabbitMqService : IRabbitMqService
{
    public void EnviarMensagem(string mensagem)
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: "fila_exemplo",
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        var body = Encoding.UTF8.GetBytes(mensagem);
        channel.BasicPublish(exchange: "",
                             routingKey: "fila_exemplo",
                             basicProperties: null,
                             body: body);
    }
}


// Classe para consumir mensagens de estoque
public class ConsumirEstoque
{
    private readonly IConfiguration _configuration;

    public ConsumirEstoque(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Start()
    {
        // Exemplo de consumo de mensagens
        var factory = new ConnectionFactory() { HostName = "localhost" };
        var connection = factory.CreateConnection();
        var channel = connection.CreateModel();

        channel.QueueDeclare(queue: "fila_exemplo",
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var mensagem = Encoding.UTF8.GetString(body);
            Console.WriteLine($"Mensagem recebida: {mensagem}");
        };

        channel.BasicConsume(queue: "fila_exemplo",
                             autoAck: true,
                             consumer: consumer);
    }
}