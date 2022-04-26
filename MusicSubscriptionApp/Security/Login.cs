using Amazon.DynamoDBv2;
using MusicSubscriptionApp.Models;

namespace MusicSubscriptionApp.Security
{
    public class Login
    {
        public static async Task<AppUser> ValidateLoginCredentials(IAmazonDynamoDB client, string email, string password)
        {
            if (!(email == null) || !(password == null))
            {
                AppUser user = await AppUser.GetAppUser(client, email);

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
