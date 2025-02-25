using BeautyVi.Core.Entities;
using BeautyVi.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

// Service for automatic cleaning of chat history
public class ChatCleanupService : IHostedService, IDisposable
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ChatCleanupService> _logger;
    private Timer _timer;

    public ChatCleanupService(IServiceScopeFactory scopeFactory, ILogger<ChatCleanupService> logger)
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
                await PerformCleanupAsync(); // Clear chat history
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing chat");
            }
        }, null, TimeSpan.Zero, TimeSpan.FromHours(24)); // Runs every 24 hours

        return Task.CompletedTask;
    }

    private async Task PerformCleanupAsync()
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<BeautyViContext>();
            var cutoffDate = DateTime.UtcNow.AddDays(-30); // Delete records older than 30 days

            var deletedCount = await context.ChatHistories
                                            .Where(ch => ch.Timestamp < cutoffDate)
                                            .ExecuteDeleteAsync(); 

            _logger.LogInformation(deletedCount > 0
                ? $"Deleted {deletedCount} old messages."
                : "No old messages found for cleanup.");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0); // Stop the timer when the service ends
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose(); // Release resources
    }
}