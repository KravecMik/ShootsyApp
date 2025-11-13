namespace Shootsy.Service
{
    public class KafkaSettings
    {
        public string BootstrapServers { get; set; } = "localhost:9092";
        public ProducerSettings ProducerSettings { get; set; } = new();
        public TopicSettings Topics { get; set; } = new();
    }

    public class ProducerSettings
    {
        public string Acks { get; set; } = "All";
        public int MessageSendMaxRetries { get; set; } = 3;
        public int RetryBackoffMs { get; set; } = 1000;
        public int LingerMs { get; set; } = 5;
        public int BatchSize { get; set; } = 32768;
    }

    public class TopicSettings
    {
        public string UserEvents { get; set; } = "user-events";
        public string FileEvents { get; set; } = "file-events";
        public string SystemEvents { get; set; } = "system-events";
        public string Messages { get; set; } = "messages";
    }
}
