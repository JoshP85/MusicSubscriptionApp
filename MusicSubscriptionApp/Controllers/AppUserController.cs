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
            ViewBag.Username = user.Username;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Dashboard([Bind("Artist, Title, Year")] Query newQuery)
        {
            var searchResult = await Query.CreateQueryFromInputAsync(client, newQuery);

            if (searchResult.Count == 0)
            {
                ModelState.AddModelError("NoResults", "No result is retrieved. Please query again");
            }
            AppUser user = AppUser.GetAppUser(dynamoDBContext, UserEmail);
            ViewBag.Username = user.Username;
            ViewBag.SearchResult = searchResult;
            return View();
        }



        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }
    }
}
