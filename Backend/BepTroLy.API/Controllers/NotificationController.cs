using Microsoft.AspNetCore.Mvc;
using BepTroLy.Application.Interfaces;
using System.Threading.Tasks;

namespace BepTroLy.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> Send([FromBody] object payload)
        {
            await _notificationService.SendNotificationAsync(payload);
            return Ok();
        }
    }
}
