using Confluent.Kafka;

namespace Shootsy.Service
{
    public interface IKafkaProducerService
    {
        Task<bool> ProduceUserEventAsync(string eventType, object userData);
        Task<bool> ProduceFileEventAsync(string eventType, object fileData);
        Task<bool> ProduceSystemEventAsync(string eventType, object systemData);
        Task<bool> ProduceAsync<T>(string topic, T message) where T : class;
    }
}
