using GameApp.Models;
using GameApp.Models.Data;
using GameApp.Models.Entities;
using GameApp.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace GameApp.Services.Repositories
{
    public class GameRegistrationRepository : IGameRegistrationRepository
    {
        private readonly GameAppDbContext _context;
        private readonly IGameConfigurationRepository _gameConfigurationRepository;
        public GameRegistrationRepository(GameAppDbContext context, IGameConfigurationRepository gameConfigurationRepository)
        {
            _context = context;
            _gameConfigurationRepository = gameConfigurationRepository;
        }



        public async Task<(IEnumerable<RegisteredGameResponseView> model, string message, bool IsSuccessful)> GetAllRegisteredGamesAsync()
        {
            try
            {
                var registeredGames = await _context.RegisteredGames
                    .Select(rg => new RegisteredGameResponseView
                    {
                        Id = rg.Id,
                        GamerName = rg.GamerName,
                        GameName = rg.GameName,
                        DurationInMinutes = rg.DurationInMinutes,
                        TotalPrice = rg.TotalPrice
                    })
                    .ToListAsync();

                return (registeredGames, "Registered games retrieved successfully", true);
            }
            catch (Exception ex)
            {
                return (null, $"Error: {ex.Message}", false);
            }
        }

        public async Task<(string message, bool IsSuccessful)> AddRegisteredGameAsync(GameRegistrationModel registration)
        {
            try
            {
                // Assuming you have a method to calculate the total price based on game configuration
                var calculatedTotalPrice = await CalculateTotalPrice(registration.GameName, registration.DurationInMinutes);

                var registeredGame = new RegisteredGame
                {
                    GamerName = registration.GamerName,
                    GameName = registration.GameName,
                    DurationInMinutes = registration.DurationInMinutes,
                    TotalPrice = calculatedTotalPrice
                };

                _context.RegisteredGames.Add(registeredGame);
                await _context.SaveChangesAsync();

                return ("Gamer registered successfully", true);
            }
            catch (Exception ex)
            {
                return ($"Error: {ex.Message}", false);
            }
        }

        public async Task<(GameRegistrationModel model, string message, bool IsSuccessful)> GetRegisteredGameByIdAsync(int id)
        {
            try
            {
                var registeredGame = await _context.RegisteredGames.FindAsync(id);

                if (registeredGame != null)
                {
                    var registeredGameModel = new GameRegistrationModel
                    {
                        Id = registeredGame.Id,
                        GamerName = registeredGame.GamerName,
                        GameName = registeredGame.GameName,
                        DurationInMinutes = registeredGame.DurationInMinutes,
                        TotalPrice = registeredGame.TotalPrice
                    };

                    return (registeredGameModel, "Registered game retrieved successfully", true);
                }
                else
                {
                    return (null, "Registered game not found", false);
                }
            }
            catch (Exception ex)
            {
                return (null, $"Error: {ex.Message}", false);
            }
        }

        public async Task<(string message, bool IsSuccessful)> UpdateRegisteredGameAsync(GameRegistrationModel registration)
        {
            try
            {
                var registeredGame = await _context.RegisteredGames.FindAsync(registration.Id);

                if (registeredGame != null)
                {
                    // Update properties of the RegisteredGame entity
                    registeredGame.GamerName = registration.GamerName;
                    registeredGame.GameName = registration.GameName;
                    registeredGame.DurationInMinutes = registration.DurationInMinutes;
                    registeredGame.TotalPrice = await CalculateTotalPrice(registration.GameName, registration.DurationInMinutes);


                    await _context.SaveChangesAsync();

                    return ("Registered game updated successfully", true);
                }
                else
                {
                    return ("Registered game not found", false);
                }
            }
            catch (Exception ex)
            {
                return ($"Error: {ex.Message}", false);
            }
        }

        private async Task<decimal> CalculateTotalPrice(string gameName, int duration)
        {
            var priceResult = await _gameConfigurationRepository.GetPricePerHourAsync(gameName);

            if (priceResult.IsSuccessful)
            {
                decimal totalPrice = priceResult.pricePerHour * duration / 60;
                return totalPrice;
            }
            else
            {
                throw new Exception("Error calculating total price");
            }
        }

    }



}
