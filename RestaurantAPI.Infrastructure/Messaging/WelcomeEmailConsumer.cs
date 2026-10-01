using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RestaurantAPI.Application;

namespace RestaurantAPI.Infrastructure.Messaging
{
    public class WelcomeEmailConsumer : BackgroundService
    {
        private readonly ILogger<WelcomeEmailConsumer> _logger;
        private readonly RabbitMqSettings _settings;

        public WelcomeEmailConsumer(ILogger<WelcomeEmailConsumer> logger, IOptions<RabbitMqSettings> options)
        {
            _settings = options.Value;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory { HostName = _settings.HostName };
            await using var connection = await factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();
            await channel.QueueDeclareAsync(queue: QueueNames.UserRegistered, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (sender, ea) =>
            {
                var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                var evt = JsonSerializer.Deserialize<UserRegisteredEvent>(json);


                _logger.LogInformation("Wysyłam maila powitalnego do {Email}", evt?.Email);
                await Task.CompletedTask;
            };

            await channel.BasicConsumeAsync(queue: QueueNames.UserRegistered, autoAck: true,
                                           consumer: consumer, cancellationToken: stoppingToken);
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }

}