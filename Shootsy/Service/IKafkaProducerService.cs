using Confluent.Kafka;

namespace Shootsy.Service
{
    public interface IKafkaProducerService
    {
        Task<bool> ProduceUserEventAsync(string eventType, object userData);
        Task<bool> ProduceFileEventAsync(string eventType, object fileData);
        Task<bool> ProduceSystemEventAsync(string eventType);
        Task<bool> ProduceAsync<T>(string topic, T message) where T : class;
        Task<bool> SendMessageByUserLoginAsync(string fromUserLogin, string toUserLogin, string messageText);
        Task<bool> ProducePostEventAsync(string eventType, object postData);
    }
}
