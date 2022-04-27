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
        public virtual ICollection<Subscription> Subscriptions { get; set; }


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

            AppUser user = new AppUser
            {
                Email = newUser.Email,
                Password = newUser.Password,
                Username = newUser.Username,
                Subscriptions = null,
            };

            await dynamoDBContext.SaveAsync(user);
            return true;
        }
    }
}
