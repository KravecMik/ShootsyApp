using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Shootsy.Service;

public class KafkaProducerService : IKafkaProducerService, IDisposable
{
    private readonly IProducer<Null, string> _producer;
    private readonly KafkaSettings _kafkaSettings;
    private readonly ILogger<KafkaProducerService> _logger;

    public KafkaProducerService(IOptions<KafkaSettings> kafkaSettings, ILogger<KafkaProducerService> logger)
    {
        _kafkaSettings = kafkaSettings.Value;
        _logger = logger;

        var config = new ProducerConfig
        {
            BootstrapServers = _kafkaSettings.BootstrapServers,
            Acks = (Acks)Enum.Parse(typeof(Acks), _kafkaSettings.ProducerSettings.Acks),
            MessageSendMaxRetries = _kafkaSettings.ProducerSettings.MessageSendMaxRetries,
            RetryBackoffMs = _kafkaSettings.ProducerSettings.RetryBackoffMs,
            LingerMs = _kafkaSettings.ProducerSettings.LingerMs,
            BatchSize = _kafkaSettings.ProducerSettings.BatchSize
        };

        _producer = new ProducerBuilder<Null, string>(config)
            .SetErrorHandler((_, error) => _logger.LogError("Kafka producer error: {Error}", error.Reason))
            .Build();
    }

    public async Task<bool> ProduceUserEventAsync(string eventType, object userData)
    {
        var message = new
        {
            EventId = Guid.NewGuid(),
            EventType = eventType,
            Timestamp = DateTime.UtcNow,
            UserId = userData
        };

        return await ProduceAsync(_kafkaSettings.Topics.UserEvents, message);
    }

    public async Task<bool> ProduceFileEventAsync(string eventType, object fileData)
    {
        var message = new
        {
            EventId = Guid.NewGuid(),
            EventType = eventType,
            Timestamp = DateTime.UtcNow,
            IdFile = fileData
        };

        return await ProduceAsync(_kafkaSettings.Topics.FileEvents, message);
    }

    public async Task<bool> ProduceSystemEventAsync(string eventType)
    {
        var message = new
        {
            EventId = Guid.NewGuid(),
            EventType = eventType,
            Timestamp = DateTime.UtcNow
        };

        return await ProduceAsync(_kafkaSettings.Topics.SystemEvents, message);
    }

    public async Task<bool> ProducePostEventAsync(string eventType, object postData)
    {
        var message = new
        {
            EventId = Guid.NewGuid(),
            EventType = eventType,
            Timestamp = DateTime.UtcNow,
            IdPost = postData
        };

        return await ProduceAsync(_kafkaSettings.Topics.UserEvents, message);
    }

    public async Task<bool> ProduceAsync<T>(string topic, T message) where T : class
    {
        try
        {
            var jsonMessage = JsonSerializer.Serialize(message, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            var result = await _producer.ProduceAsync(topic, new Message<Null, string> { Value = jsonMessage });

            _logger.LogInformation("Message delivered to {Topic} at offset {Offset}",
                result.Topic, result.Offset);

            return result.Status == PersistenceStatus.Persisted;
        }
        catch (ProduceException<Null, string> ex)
        {
            _logger.LogError(ex, "Failed to deliver message to Kafka. Error: {Error}", ex.Error.Reason);
            return false;
        }
    }

    public async Task<bool> SendMessageByUserLoginAsync(string fromUserLogin, string toUserLogin, string messageText)
    {
        var message = new
        {
            EventId = Guid.NewGuid(),
            CreateDate = DateTime.UtcNow,
            From = fromUserLogin,
            To = toUserLogin,
            Message = messageText
        };

        return await ProduceAsync(_kafkaSettings.Topics.Messages, message);
    }

    public void Dispose()
    {
        _producer?.Flush(TimeSpan.FromSeconds(10));
        _producer?.Dispose();
    }
}