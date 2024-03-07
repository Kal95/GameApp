using GameApp.Models;

namespace GameApp.Services.Interface
{
    public interface IGameRegistrationRepository
    {
        Task<(IEnumerable<RegisteredGameResponseView> model, string message, bool IsSuccessful)> GetAllRegisteredGamesAsync();
        Task<(string message, bool IsSuccessful)> AddRegisteredGameAsync(GameRegistrationModel registration);
        Task<(GameRegistrationModel model, string message, bool IsSuccessful)> GetRegisteredGameByIdAsync(int id);
        Task<(string message, bool IsSuccessful)> UpdateRegisteredGameAsync(GameRegistrationModel registration);
       
    }
}
