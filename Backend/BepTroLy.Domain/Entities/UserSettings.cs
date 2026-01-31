namespace BepTroLy.Domain.Entities
{
    public class UserSettings
    {
        public int SettingId { get; set; }
        public int UserId { get; set; }
        public int NotifyBeforeDays { get; set; }
        public bool AllowNotification { get; set; }
        public string? Language { get; set; }
    }
}
