using System.ComponentModel.DataAnnotations;

namespace GameApp.Models
{
    public class GameConfigurationResponseView
    {
        public int Id { get; set; }
        public string GameName { get; set; }
        public decimal PricePerHour { get; set; }
    }
}
