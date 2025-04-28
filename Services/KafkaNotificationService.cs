using System;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;

namespace bloodsociety.Services
{
    public class KafkaNotificationService
    {
        private readonly string _bootstrapServers;
        private readonly string _topic;

        public KafkaNotificationService(IConfiguration configuration)
        {
            _bootstrapServers = configuration["Kafka:BootstrapServers"];
            _topic = configuration["Kafka:NotificationTopic"];
        }

        public async Task SendNotificationAsync(string message)
        {
            var config = new ProducerConfig { BootstrapServers = _bootstrapServers };
            using var producer = new ProducerBuilder<Null, string>(config).Build();
            await producer.ProduceAsync(_topic, new Message<Null, string> { Value = message });
        }
    }
}