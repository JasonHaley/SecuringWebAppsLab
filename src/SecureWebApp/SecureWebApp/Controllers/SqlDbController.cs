using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SecureWebApp.Models;
using SecureWebApp.Services;

namespace SecureWebApp.Controllers
{
    public class SqlDbController : Controller
    {
        private readonly IDemoService _demoService;

        public SqlDbController(IDemoService demoService)
        {
            _demoService = demoService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Ef()
        {
            SqlDbViewModel model = await _demoService.AccessEFDatabase();
            return View(model);
        }

        public async Task<IActionResult> Ado()
        {
            SqlDbViewModel model = await _demoService.AccessAdoSqlDatabase();
            return View(model);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
