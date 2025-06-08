using Confluent.Kafka;
using System.Text.Json;
using KafkaCacheConsumer;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly List<Product> _cache;
    private readonly string _bootstrapServers = "host.docker.internal:9092";
    private readonly string _topic = "product-cache";
    private readonly string _groupId = "product-cache-consumer-group";

    public Worker(ILogger<Worker> logger, List<Product> cache)
    {
        _logger = logger;
        _cache = cache;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(() =>
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _bootstrapServers,
                GroupId = _groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe(_topic);

            _logger.LogInformation("Kafka consumer started. Listening to topic: {Topic}", _topic);

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var result = consumer.Consume(stoppingToken);
                        var product = JsonSerializer.Deserialize<Product>(result.Message.Value);
                        if (product != null)
                        {
                            _cache.Add(product);
                            _logger.LogInformation("Cached product: {Name} ({Id})", product.Name, product.Id);
                        }
                    }
                    catch (ConsumeException ex)
                    {
                        _logger.LogError(ex, "Consume error: {Reason}", ex.Error.Reason);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Kafka consumer shutting down...");
                consumer.Close();
            }
        }, stoppingToken);
    }
}