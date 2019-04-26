using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SecureWebApp.Models;
using SecureWebApp.Options;
using SecureWebApp.Services;

namespace SecureWebApp.Controllers
{
    public class KeyVaultController : Controller
    {
        private readonly IDemoService _demoService;
        private readonly LabSettings _settings;

        public KeyVaultController(IDemoService demoService, IOptionsSnapshot<LabSettings> labSettings)
        {
            _demoService = demoService;
            _settings = labSettings.Value;
        }

        [HttpGet, HttpHead]
        public IActionResult Index()
        {
            var model = new KeyVaultViewModel
            {
                SecretValue = _settings.KeyVaultSecret
            };
            return View(model);
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
