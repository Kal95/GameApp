namespace GameApp.Models
{
    public class GameRegistrationModel
    {
        public int Id { get; set; }
        public string GamerName { get; set; }
        public string GameName { get; set; }
        public int DurationInMinutes { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
