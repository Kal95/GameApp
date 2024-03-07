using GameApp.Models;
using GameApp.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace GameApp.Controllers
{
    public class GamerRegistrationController : Controller
    {
        private readonly IGameRegistrationRepository _gameRegistrationRepository;
        private readonly IGameConfigurationRepository _gameConfigurationRepository;

        public GamerRegistrationController(IGameRegistrationRepository gamerRegistrationRepository, IGameConfigurationRepository gameConfigurationRepository)
        {
            _gameRegistrationRepository = gamerRegistrationRepository;
            _gameConfigurationRepository = gameConfigurationRepository;
        }

        public async Task<IActionResult> GamerIndex()
        {
            var registrations = await _gameRegistrationRepository.GetAllRegisteredGamesAsync();
            var gameRegistrationModels = registrations.model
                .Select(rg => new GameRegistrationModel
                {
                    Id = rg.Id,
                    GamerName = rg.GamerName,
                    GameName = rg.GameName,
                    DurationInMinutes = rg.DurationInMinutes,
                    TotalPrice = rg.TotalPrice
                });

            return View(gameRegistrationModels);
        }


        public async Task<IActionResult> RegisterGamer()
        {
            var gameConfigurations = await _gameConfigurationRepository.GetAllConfigurationsAsync();
            ViewBag.GameConfigurations = gameConfigurations.model;

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> RegisterGamer(GameRegistrationModel registration)
        {
            var result = await _gameRegistrationRepository.AddRegisteredGameAsync(registration);

            if (result.IsSuccessful)
                return RedirectToAction("Index");
            else
                return BadRequest(result);
        }

        [HttpPost]
        public async Task<IActionResult> CalculateTotalPrice(string gameName, int duration)
        {
            // Implement logic to calculate total price based on game configuration
            var priceResult = await _gameConfigurationRepository.GetPricePerHourAsync(gameName);

            if (priceResult.IsSuccessful)
            {
                decimal totalPrice = priceResult.pricePerHour * duration / 60; 
                return Json(totalPrice);
            }
            else
            {
                return Json("Error calculating total price");
            }
        }


        [HttpGet]
        public async Task<IActionResult> EditGamer(int id)
        {
            var result = await _gameRegistrationRepository.GetRegisteredGameByIdAsync(id);

            if (result.IsSuccessful)
            {
                // Populate ViewBag with Game Configurations for display
                var gameConfigurations = await _gameConfigurationRepository.GetAllConfigurationsAsync();
                ViewBag.GameConfigurations = gameConfigurations.model;

                return View(result.model);
            }
            else
            {
                return NotFound(result);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditGamer(GameRegistrationModel registration)
        {
            var result = await _gameRegistrationRepository.UpdateRegisteredGameAsync(registration);

            if (result.IsSuccessful)
                return RedirectToAction("Index");
            else
                return BadRequest(result);
        }
    }
}
