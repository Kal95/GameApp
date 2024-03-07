using GameApp.Models;
using GameApp.Models.Data;
using GameApp.Models.Entities;
using GameApp.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace GameApp.Services.Repositories
{
    public class GameConfigurationRepository : IGameConfigurationRepository
    {
        private readonly GameAppDbContext _context;
        public GameConfigurationRepository(GameAppDbContext context)
        {
            _context = context;
        }


        public async Task<(IEnumerable<GameConfigurationResponseView> model, string message, bool IsSuccessful)> GetAllConfigurationsAsync()
        {
            try
            {
                var configurations = await _context.GameConfigurations
                    .Select(c => new GameConfigurationResponseView
                    {
                        Id = c.Id,
                        GameName = c.GameName,
                        PricePerHour = c.PricePerHour,

                    })
                    .ToListAsync();

                return (configurations, "Configurations retrieved successfully", true);
            }
            catch (Exception ex)
            {
                return (null, $"Error: {ex.Message}", false);
            }
        }

        public async Task<(GameConfigurationModel model, string message, bool IsSuccessful)> GetConfigurationByIdAsync(int id)
        {
            try
            {
                var configuration = await _context.GameConfigurations
                    .Where(c => c.Id == id)
                    .Select(c => new GameConfigurationModel
                    {
                        Id = c.Id,
                        GameName = c.GameName,
                        PricePerHour = c.PricePerHour,
                        // Include other properties as needed
                    })
                    .FirstOrDefaultAsync();

                if (configuration != null)
                    return (configuration, "Configuration retrieved successfully", true);
                else
                    return (null, "Configuration not found", false);
            }
            catch (Exception ex)
            {
                return (null, $"Error: {ex.Message}", false);
            }
        }


        public async Task<(GameConfigurationResponseView model, string message, bool IsSuccessful)> AddConfigurationAsync(GameConfigurationModel configuration)
        {
            try
            {
                var entity = new GameConfiguration
                {
                    GameName = configuration.GameName,
                    PricePerHour = configuration.PricePerHour,
                    // Map other properties
                };

                _context.GameConfigurations.Add(entity);
                await _context.SaveChangesAsync();

                var responseModel = new GameConfigurationResponseView
                {
                    Id = entity.Id,
                    GameName = entity.GameName,
                    PricePerHour = entity.PricePerHour,
                    // Map other properties
                };

                return (responseModel, "Configuration added successfully", true);
            }
            catch (Exception ex)
            {
                return (null, $"Error: {ex.Message}", false);
            }
        }

        public async Task<(string message, bool IsSuccessful)> UpdateConfigurationAsync(GameConfigurationModel configuration)
        {
            try
            {
                var entity = await _context.GameConfigurations.FindAsync(configuration.Id);

                if (entity != null)
                {
                    entity.GameName = configuration.GameName;
                    entity.PricePerHour = configuration.PricePerHour;
                   

                    await _context.SaveChangesAsync();
                    return ("Configuration updated successfully", true);
                }
                else
                {
                    return ("Configuration not found", false);
                }
            }
            catch (Exception ex)
            {
                return ($"Error: {ex.Message}", false);
            }
        }

        public async Task<(string message, bool IsSuccessful)> DeleteConfigurationAsync(int id)
        {
            try
            {
                var configuration = await _context.GameConfigurations.FindAsync(id);

                if (configuration != null)
                {
                    _context.GameConfigurations.Remove(configuration);
                    await _context.SaveChangesAsync();
                    return ("Configuration deleted successfully", true);
                }
                else
                {
                    return ("Configuration not found", false);
                }
            }
            catch (Exception ex)
            {
                return ($"Error: {ex.Message}", false);
            }
        }

        public async Task<(decimal pricePerHour, string message, bool IsSuccessful)> GetPricePerHourAsync(string gameName)
        {
            try
            {
                var configuration = await _context.GameConfigurations
                    .SingleOrDefaultAsync(c => c.GameName == gameName);

                if (configuration != null)
                {
                    return (configuration.PricePerHour, "Price per hour retrieved successfully", true);
                }
                else
                {
                    return (0, "Game configuration not found", false);
                }
            }
            catch (Exception ex)
            {
                return (0, $"Error: {ex.Message}", false);
            }
        }

    }
}
