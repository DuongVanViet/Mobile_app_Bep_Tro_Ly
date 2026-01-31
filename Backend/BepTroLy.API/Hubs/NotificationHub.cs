using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace BepTroLy.API.Hubs
{
    public class NotificationHub : Hub
    {
        private static readonly Dictionary<string, string> UserConnections = new();

        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                UserConnections[userId] = Context.ConnectionId;
                await Clients.All.SendAsync("UserOnline", userId);
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userId = Context.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                UserConnections.Remove(userId);
                await Clients.All.SendAsync("UserOffline", userId);
            }
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendNotification(string userId, string message, string type)
        {
            if (UserConnections.TryGetValue(userId, out var connectionId))
            {
                await Clients.Client(connectionId).SendAsync("ReceiveNotification", new
                {
                    Message = message,
                    Type = type,
                    Timestamp = DateTime.UtcNow
                });
            }
        }

        public async Task SendExpiryAlert(string userId, string ingredientName, string expiryDate)
        {
            if (UserConnections.TryGetValue(userId, out var connectionId))
            {
                await Clients.Client(connectionId).SendAsync("ExpiryAlert", new
                {
                    IngredientName = ingredientName,
                    ExpiryDate = expiryDate,
                    AlertTime = DateTime.UtcNow
                });
            }
        }

        public async Task SendMealPlanNotification(string userId, string recipeName, string mealType, string plannedDate)
        {
            if (UserConnections.TryGetValue(userId, out var connectionId))
            {
                await Clients.Client(connectionId).SendAsync("MealPlanUpdate", new
                {
                    RecipeName = recipeName,
                    MealType = mealType,
                    PlannedDate = plannedDate,
                    NotificationTime = DateTime.UtcNow
                });
            }
        }

        public async Task JoinUserGroup(string userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");
        }

        public async Task LeaveUserGroup(string userId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user_{userId}");
        }
    }
}
