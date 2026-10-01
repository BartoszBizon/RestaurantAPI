using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RestaurantAPI.Application.Interfaces;

namespace RestaurantAPI.Infrastructure
{
    public class RabbitMqPublisher : IMessagePublisher
    {

        private readonly RabbitMqSettings _settings;

        public RabbitMqPublisher(IOptions<RabbitMqSettings> options)
        {
            _settings = options.Value;
        }
        public async Task PublishAsync<T>(T message, string queueName)
        {

            var factory = new ConnectionFactory { HostName = _settings.HostName };
            await using var connection = await factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();
            await channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);
            await channel.BasicPublishAsync(exchange: string.Empty, routingKey: queueName, body: body);
        }
    }
}