using System;

namespace BepTroLy.Domain.Entities
{
    public class ExpiryNotification
    {
        public int NotificationId { get; set; }
        public int UserIngredientId { get; set; }
        public DateTime NotifyDate { get; set; }
        public bool IsSent { get; set; }
        public DateTime CreatedAt { get; set; }
        
        // Navigation property
        public UserIngredient UserIngredient { get; set; }
    }
}
