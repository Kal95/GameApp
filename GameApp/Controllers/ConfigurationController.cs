using GameApp.Models;
using GameApp.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace GameApp.Controllers
{
    public class ConfigurationController : Controller
    {
        private readonly IGameConfigurationRepository _gameConfigurationRepository;

        public ConfigurationController(IGameConfigurationRepository gameConfigurationRepository)
        {
            _gameConfigurationRepository = gameConfigurationRepository;
        }

        public async Task<IActionResult> ConfigurationIndex()
        {
            var configurations = await _gameConfigurationRepository.GetAllConfigurationsAsync();
            return View(configurations.model);
        }


        public IActionResult AddConfiguration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddConfiguration(GameConfigurationModel configuration)
        {
            var result = await _gameConfigurationRepository.AddConfigurationAsync(configuration);

            if (result.IsSuccessful)
                return RedirectToAction("ConfigurationIndex");
            else
                return BadRequest(result);
        }

        [HttpGet]
        public async Task<IActionResult> EditConfiguration(int id)
        {
            var result = await _gameConfigurationRepository.GetConfigurationByIdAsync(id);

            if (result.IsSuccessful)
                return View(result.model);
            else
                return NotFound(result);
        }

        [HttpPost]
        public async Task<IActionResult> EditConfiguration(GameConfigurationModel configuration)
        {
            var result = await _gameConfigurationRepository.UpdateConfigurationAsync(configuration);

            if (result.IsSuccessful)
                return RedirectToAction("ConfigurationIndex");
            else
                return BadRequest(result);
        }

        //[HttpGet]
        //public async Task<IActionResult> DeleteConfiguration(int id)
        //{
        //    var result = await _gameConfigurationRepository.GetConfigurationByIdAsync(id);

        //    if (result.IsSuccessful)
        //        return View(result.model);
        //    else
        //        return NotFound(result);
        //}

        [HttpPost]
        public async Task<IActionResult> DeleteConfiguration(int id)
        {
            var result = await _gameConfigurationRepository.DeleteConfigurationAsync(id);

            if (result.IsSuccessful)
                return Json(new { IsSuccessful = true });
            else
                return Json(new { IsSuccessful = false, Message = result.message });
        }


    }
}
