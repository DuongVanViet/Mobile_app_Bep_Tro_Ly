using System.Threading.Tasks;

namespace BepTroLy.Application.Interfaces
{
    public interface INotificationService
    {
        Task SendNotificationAsync(object payload);
    }
}
