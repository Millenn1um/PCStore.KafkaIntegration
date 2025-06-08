using Confluent.Kafka;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace KafkaCacheDistributor;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConfiguration _config;
    private readonly IProducer<Null, string> _kafkaProducer;
    private readonly string _sqlConnectionString;
    private readonly string _kafkaTopic;

    public Worker(ILogger<Worker> logger, IConfiguration config)
    {
        _logger = logger;
        _config = config;

        _sqlConnectionString = _config.GetConnectionString("SqlDb")!;
        _kafkaTopic = _config["Kafka:Topic"]!;

        var kafkaConfig = new ProducerConfig
        {
            BootstrapServers = _config["Kafka:BootstrapServers"]
        };

        _kafkaProducer = new ProducerBuilder<Null, string>(kafkaConfig).Build();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var connection = new SqlConnection(_sqlConnectionString);
                var cpus = await connection.QueryAsync("SELECT Id, Name, Price FROM CPUs");

                foreach (var cpu in cpus)
                {
                    var json = JsonSerializer.Serialize(cpu);
                    _logger.LogInformation("Published to Kafka: {Json}", (object)json);

                    await _kafkaProducer.ProduceAsync(_kafkaTopic, new Message<Null, string> { Value = json }, stoppingToken);
                }

                _logger.LogInformation("Cycle complete. Waiting 30 seconds...");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during execution cycle");
            }

            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }

    public override void Dispose()
    {
        _kafkaProducer?.Dispose();
        base.Dispose();
    }
}