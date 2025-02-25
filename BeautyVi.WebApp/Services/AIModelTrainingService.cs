using BeautyVi.Core.Entities;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

// Model training service
public class AIModelTrainingService : IHostedService, IDisposable
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<AIModelTrainingService> _logger;
    private Timer _timer;
    private readonly string _ollamaUrl = "http://localhost:11434/api/generate";
    private const int BatchSize = 100; // Number of chats in the package

    public AIModelTrainingService(IServiceScopeFactory scopeFactory, ILogger<AIModelTrainingService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(async _ =>
        {
            try
            {
                await TrainModelAsync(); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while training the model");
            }
        }, null, TimeSpan.Zero, TimeSpan.FromHours(24)); // Run interval every 24 hours

        return Task.CompletedTask;
    }

    private async Task TrainModelAsync()
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BeautyViContext>();

        try
        {
            // New chats for the last day
            var lastTrainingDate = DateTime.UtcNow.AddDays(-1);
            var recentChats = context.ChatHistories
                .Where(ch => ch.Timestamp > lastTrainingDate)
                .ToList();

            if (!recentChats.Any())
            {
                _logger.LogInformation("No new data to train on.");
                return;
            }

            // Filtering duplicates and "garbage"
            var filteredChats = recentChats
                .GroupBy(ch => new { ch.UserMessage, ch.AIResponse }) // Видаляємо дублі
                .Select(g => g.First())
                .Where(ch => !string.IsNullOrWhiteSpace(ch.UserMessage) && !string.IsNullOrWhiteSpace(ch.AIResponse)) // Прибираємо порожні повідомлення
                .ToList();

            _logger.LogInformation($"Prepared {filteredChats.Count} unique records for learning.");

            // Batch processing
            for (int i = 0; i < filteredChats.Count; i += BatchSize)
            {
                var batch = filteredChats.Skip(i).Take(BatchSize).ToList();

                var aiTrainingRequest = new
                {
                    model = "mistral",
                    data = batch.Select(ch => new
                    {
                        prompt = ch.UserMessage,
                        response = ch.AIResponse
                    })
                };

                var jsonTrainingRequest = JsonSerializer.Serialize(aiTrainingRequest);
                var content = new StringContent(jsonTrainingRequest, Encoding.UTF8, "application/json");

                // Sending the package + error handling
                using var httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };

                bool success = false;
                int retries = 3;

                while (!success && retries > 0)
                {
                    try
                    {
                        var response = await httpClient.PostAsync(_ollamaUrl, content);
                        var responseString = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode)
                        {
                            _logger.LogInformation($"Package {i / BatchSize + 1} sent successfully. Response: {responseString}");
                            success = true;
                        }
                        else
                        {
                            _logger.LogWarning($"An error occurred when sending the package: {response.StatusCode}. There are still attempts: {retries - 1}");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"An attempt to send the package failed. There are still attempts: {retries - 1}");
                    }

                    retries--;
                    if (!success && retries > 0)
                    {
                        await Task.Delay(2000); // Delay before repeating
                    }
                }

                if (!success)
                {
                    _logger.LogError($"The package could not be sent {i / BatchSize + 1} after 3 attempts.");
                }

                // A small pause between packets to avoid overloading the API
                await Task.Delay(500);
            }

            _logger.LogInformation("Model training is complete.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Global error during model training.");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose(); // Release resources
    }
}
