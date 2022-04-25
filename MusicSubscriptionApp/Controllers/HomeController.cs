using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.AspNetCore.Mvc;
using MusicSubscriptionApp.Data;
using MusicSubscriptionApp.Models;
using MusicSubscriptionApp.Security;
using System.Diagnostics;


namespace MusicSubscriptionApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDynamoDBContext dynamoDBContext;
        private readonly IAmazonDynamoDB client;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IDynamoDBContext dynamoDBContext, IAmazonDynamoDB client, ILogger<HomeController> logger)
        {
            this.dynamoDBContext = dynamoDBContext;
            this.client = client;
            _logger = logger;
        }

        public async Task<IActionResult> IndexAsync()
        {
            await CreateTables.CreateMusicTableAsync(client);

            await CreateTables.CreateLoginTableAsync(client);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> IndexAsync(string email, string password)
        {
            User user = await Login.ValidateLoginCredentials(client, email, password);

            if (user != null)
            {
                HttpContext.Session.SetString(nameof(Models.User.Email), user.Email);
                return RedirectToAction("Dashboard", "User");
            }

            ModelState.AddModelError("LoginFailed", "Email or Password is invalid.");

            return View();
        }




        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}