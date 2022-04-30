using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.S3;
using Microsoft.AspNetCore.Mvc;
using MusicSubscriptionApp.Data;
using MusicSubscriptionApp.Models;
using System.Diagnostics;


namespace MusicSubscriptionApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDynamoDBContext dynamoDBContext;
        private readonly IAmazonDynamoDB client;
        private readonly IAmazonS3 clientS3;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IDynamoDBContext dynamoDBContext, IAmazonDynamoDB client, IAmazonS3 clientS3, ILogger<HomeController> logger)
        {
            this.dynamoDBContext = dynamoDBContext;
            this.client = client;
            this.clientS3 = clientS3;
            _logger = logger;
        }

        public async Task<IActionResult> IndexAsync()
        {

            await CreateTables.CreateMusicTableAsync(client, clientS3);

            await CreateTables.CreateLoginTableAsync(client);

            return View();
        }

        [HttpPost]
        public IActionResult Index(string email, string password)
        {
            if (email is not null)
            {
                AppUser user = AppUser.GetAppUser(dynamoDBContext, email);

                if (user is not null)
                {
                    if (user.Password == password)
                    {
                        HttpContext.Session.SetString(nameof(AppUser.Email), user.Email);
                        return RedirectToAction("Dashboard", "AppUser");
                    }
                }
            }

            ModelState.AddModelError("LoginFailed", "Email or Password is invalid.");

            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register([Bind("Email, Username, Password")] AppUser newUser)
        {
            var isSuccessful = await AppUser.CreateAppUser(dynamoDBContext, newUser);
            if (isSuccessful)
            {
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("RegisterFailed", "The email already exists.");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}