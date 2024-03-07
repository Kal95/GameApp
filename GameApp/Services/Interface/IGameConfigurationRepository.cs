using GameApp.Models;
using GameApp.Models.Entities;

namespace GameApp.Services.Interface
{
    public interface IGameConfigurationRepository
    {
        Task<(IEnumerable<GameConfigurationResponseView> model, string message, bool IsSuccessful)> GetAllConfigurationsAsync();
        Task<(GameConfigurationModel model, string message, bool IsSuccessful)> GetConfigurationByIdAsync(int id);
        Task<(GameConfigurationResponseView model, string message, bool IsSuccessful)> AddConfigurationAsync(GameConfigurationModel configuration);
        Task<(string message, bool IsSuccessful)> UpdateConfigurationAsync(GameConfigurationModel configuration);
        Task<(string message, bool IsSuccessful)> DeleteConfigurationAsync(int id);
        Task<(decimal pricePerHour, string message, bool IsSuccessful)> GetPricePerHourAsync(string gameName);
    }
}
