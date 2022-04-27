using A1T1s3655612.Security;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.AspNetCore.Mvc;
using MusicSubscriptionApp.Models;

namespace MusicSubscriptionApp.Controllers
{
    [RequireSession]
    public class AppUserController : Controller
    {
        private string UserEmail => HttpContext.Session.GetString(nameof(AppUser.Email));
        private readonly IDynamoDBContext dynamoDBContext;
        private readonly IAmazonDynamoDB client;

        public AppUserController(IDynamoDBContext dynamoDBContext, IAmazonDynamoDB client)
        {
            this.dynamoDBContext = dynamoDBContext;
            this.client = client;
        }

        public IActionResult Dashboard()
        {
            AppUser user = AppUser.GetAppUser(dynamoDBContext, UserEmail);

            return View(user);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }
    }
}
