using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.Application.Interfaces
{
    public interface IMessagePublisher
    {
        Task PublishAsync<T>(T message, string queueName);
    }
}