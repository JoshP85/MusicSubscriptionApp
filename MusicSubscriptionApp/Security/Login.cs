using Amazon.DynamoDBv2;
using MusicSubscriptionApp.Models;
using MusicSubscriptionApp.Services;

namespace MusicSubscriptionApp.Security
{
    public class Login
    {
        public static async Task<AppUser> ValidateLoginCredentials(IAmazonDynamoDB client, string email, string password)
        {
            if (!(email == null) || !(password == null))
            {
                AppUser user = await AppUserControllerServices.GetAppUser(client, email);

                if (user != null)
                {
                    if (user.Password == password)
                        return user;
                }
            }
            return null;
        }
    }
}
