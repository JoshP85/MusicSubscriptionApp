using Amazon.DynamoDBv2.DataModel;
using System.ComponentModel.DataAnnotations;

namespace MusicSubscriptionApp.Models
{
    [DynamoDBTable("login")]
    public class AppUser
    {
        [DynamoDBHashKey]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [DynamoDBProperty("User_Name")]
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [DynamoDBProperty]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [DynamoDBProperty]
        public List<string> Subscriptions { get; set; }


        public static AppUser GetAppUser(IDynamoDBContext dynamoDBContext, string email)
        {
            return dynamoDBContext.LoadAsync<AppUser>(email).Result;
        }

        public static async Task<bool> CreateAppUser(IDynamoDBContext dynamoDBContext, AppUser newUser)
        {
            if (GetAppUser(dynamoDBContext, newUser.Email) is not null)
            {
                return false;
            }

            AppUser appUser = new AppUser
            {
                Email = newUser.Email,
                Password = newUser.Password,
                Username = newUser.Username,
                Subscriptions = new List<string> { },
            };

            await dynamoDBContext.SaveAsync(appUser);
            return true;
        }

        public static void NewSubscription(string songID, string email, IDynamoDBContext dynamoDBContext)
        {
            AppUser appUser = GetAppUser(dynamoDBContext, email);

            var subList = new List<string>();

            subList.Add(songID);

            if (appUser.Subscriptions != null)
            {
                subList.AddRange(appUser.Subscriptions);
            }

            appUser.Subscriptions = subList;

            dynamoDBContext.SaveAsync(appUser);
        }


        public static List<Song> GetSubscriptionList(AppUser appUser, IDynamoDBContext dynamoDBContext)
        {
            if (appUser.Subscriptions == null)
            {
                return null;
            }
            var subList = new List<Song>();
            foreach (var subscription in appUser.Subscriptions)
            {
                var song = dynamoDBContext.LoadAsync<Song>(subscription).Result;
                subList.Add(song);
            }

            return subList;
        }
    }
}
