using System.Threading.Tasks;
using BepTroLy.Application.Interfaces;
using BepTroLy.Infrastructure.Notification;

namespace BepTroLy.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly FirebasePushService _firebase;

        public NotificationService(FirebasePushService firebase)
        {
            _firebase = firebase;
        }

        public Task SendNotificationAsync(object payload)
        {
            return _firebase.SendAsync(payload);
        }
    }
}
