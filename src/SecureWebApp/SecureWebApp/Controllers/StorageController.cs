using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SecureWebApp.Models;
using SecureWebApp.Services;

namespace SecureWebApp.Controllers
{
    public class StorageController : Controller
    {
        private readonly IDemoService _demoService;

        public StorageController(IDemoService demoService)
        {
            _demoService = demoService;
        }

        [HttpGet, HttpHead]
        public async Task<IActionResult> Index()
        {
            StorageViewModel model = await _demoService.AccessStorage();

            return View(model);
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
