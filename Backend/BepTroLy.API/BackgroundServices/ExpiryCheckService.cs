using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.SignalR;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BepTroLy.Infrastructure.Persistence;
using BepTroLy.Domain.Entities;
using BepTroLy.API.Hubs;

namespace BepTroLy.API.BackgroundServices
{
    public class ExpiryCheckService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private IHubContext<NotificationHub> _hubContext;

        public ExpiryCheckService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Wait a bit for app to fully start
            await Task.Delay(5000, stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Get hub context on each iteration
                    _hubContext = _serviceProvider.GetRequiredService<IHubContext<NotificationHub>>();
                    await CheckAndNotifyExpiringIngredientsAsync();
                }
                catch (Exception ex)
                {
                    // Log error but don't stop the service
                    Console.WriteLine($"Error in ExpiryCheckService: {ex.Message}");
                }

                // Check every 24 hours
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }

        private async Task CheckAndNotifyExpiringIngredientsAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                // Get today's date
                var today = DateTime.UtcNow.Date;
                var threeMonthsFromNow = today.AddDays(3);

                // Find ingredients expiring in next 3 days that don't have a notification yet
                var expiringIngredients = await dbContext.UserIngredients
                    .Include(ui => ui.User)
                    .Include(ui => ui.Ingredient)
                    .Where(ui => ui.ExpiryDate.HasValue &&
                                 ui.ExpiryDate.Value.Date >= today &&
                                 ui.ExpiryDate.Value.Date <= threeMonthsFromNow)
                    .ToListAsync();

                var now = DateTime.UtcNow;

                foreach (var ingredient in expiringIngredients)
                {
                    // Check if notification already exists
                    var existingNotification = await dbContext.ExpiryNotifications
                        .FirstOrDefaultAsync(n => n.UserIngredientId == ingredient.UserIngredientId && !n.IsSent);

                    if (existingNotification == null)
                    {
                        // Create new notification
                        var notification = new ExpiryNotification
                        {
                            UserIngredientId = ingredient.UserIngredientId,
                            NotifyDate = ingredient.ExpiryDate.Value.Date,
                            IsSent = false,
                            CreatedAt = now
                        };

                        dbContext.ExpiryNotifications.Add(notification);
                    }
                }

                await dbContext.SaveChangesAsync();

                // Get unsent notifications that are due
                var dueNotifications = await dbContext.ExpiryNotifications
                    .Include(n => n.UserIngredient)
                        .ThenInclude(ui => ui.User)
                    .Include(n => n.UserIngredient)
                        .ThenInclude(ui => ui.Ingredient)
                    .Where(n => !n.IsSent && n.NotifyDate <= today)
                    .ToListAsync();

                foreach (var notification in dueNotifications)
                {
                    // Mark as sent
                    notification.IsSent = true;

                    // Send real-time notification via SignalR
                    if (_hubContext != null)
                    {
                        try
                        {
                            await _hubContext.Clients.Group($"user_{notification.UserIngredient.UserId}")
                                .SendAsync("ExpiryAlert", new
                                {
                                    IngredientName = notification.UserIngredient.Ingredient.IngredientName,
                                    ExpiryDate = notification.UserIngredient.ExpiryDate?.ToString("yyyy-MM-dd"),
                                    AlertTime = DateTime.UtcNow
                                });
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error sending SignalR notification: {ex.Message}");
                        }
                    }

                    Console.WriteLine($"[EXPIRY ALERT] User {notification.UserIngredient.UserId}: " +
                        $"{notification.UserIngredient.Ingredient.IngredientName} expires on " +
                        $"{notification.UserIngredient.ExpiryDate:yyyy-MM-dd}");
                }

                if (dueNotifications.Any())
                {
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}

