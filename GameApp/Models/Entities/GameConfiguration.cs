using System.ComponentModel.DataAnnotations;

namespace GameApp.Models.Entities
{
    public class GameConfiguration
    {

        public int Id { get; set; }
        [Required]
        public string GameName { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Please enter a valid price.")]
        public decimal PricePerHour { get; set; }
    }
}
