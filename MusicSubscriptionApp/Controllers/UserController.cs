using Microsoft.AspNetCore.Mvc;

namespace MusicSubscriptionApp.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
