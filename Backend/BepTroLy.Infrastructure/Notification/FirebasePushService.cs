using System.Threading.Tasks;

namespace BepTroLy.Infrastructure.Notification
{
    public class FirebasePushService
    {
        public Task SendAsync(object payload)
        {
            // Placeholder: integrate Firebase Admin SDK or HTTP API
            return Task.CompletedTask;
        }
    }
}
