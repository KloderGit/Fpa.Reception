using Microsoft.AspNetCore.Mvc;

namespace reception.fitnesspro.ru.Controllers.Settings
{
    public class SettingsController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}