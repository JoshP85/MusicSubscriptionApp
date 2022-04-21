using Microsoft.AspNetCore.Mvc;

namespace MusicSubscriptionApp.Controllers
{
    public class SubscriptionController : Controller
    {
        // GET: SubscriptionController
        public ActionResult Index()
        {
            return View();
        }
    }
}
