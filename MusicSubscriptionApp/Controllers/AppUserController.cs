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
            AppUser appUser = AppUser.GetAppUser(dynamoDBContext, UserEmail);

            ViewBag.Subscriptions = AppUser.GetSubscriptionList(appUser, dynamoDBContext);

            ViewBag.AppUser = appUser;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Dashboard([Bind("Artist, Title, Year")] Query newQuery)
        {
            AppUser appUser = AppUser.GetAppUser(dynamoDBContext, UserEmail);

            ViewBag.AppUser = appUser;

            ViewBag.Subscriptions = AppUser.GetSubscriptionList(appUser, dynamoDBContext);

            var queryResult = await Query.CreateQueryFromInputAsync(client, newQuery);

            if (queryResult == null)
                return View();

            if (queryResult.Count > 0)
                ViewBag.SearchResult = queryResult;

            if (queryResult.Count == 0)
                ModelState.AddModelError("NoResults", "No result is retrieved. Please query again");

            return View();
        }


        public IActionResult NewSubscription([Bind("SongID")] Song newSubscription, AppUser currentAppUser)
        {
            AppUser.NewSubscription(newSubscription.SongID, currentAppUser.Email, dynamoDBContext);

            return RedirectToAction("Dashboard", "AppUser");
        }

        public IActionResult RemoveSubscription([Bind("SongID")] Song subscription, AppUser currentAppUser)
        {
            AppUser.RemoveSubscription(subscription.SongID, currentAppUser.Email, dynamoDBContext);

            return RedirectToAction("Dashboard", "AppUser");
        }



        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }
    }
}
