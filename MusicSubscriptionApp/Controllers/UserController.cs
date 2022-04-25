using A1T1s3655612.Security;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.AspNetCore.Mvc;
using MusicSubscriptionApp.AWS;

namespace MusicSubscriptionApp.Controllers
{
    [RequireSession]
    public class UserController : Controller
    {
        private readonly IDynamoDBContext dynamoDBContext;
        private readonly IAmazonDynamoDB client;

        public UserController(IDynamoDBContext dynamoDBContext, IAmazonDynamoDB client)
        {
            this.dynamoDBContext = dynamoDBContext;
            this.client = client;
        }

        private string userEmail => HttpContext.Session.GetString(nameof(Models.User.Email));
        public IActionResult Dashboard()
        {
            Models.User user = DynamoDB.GetUser(client, userEmail).Result;

            return View(user);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }
    }
}
