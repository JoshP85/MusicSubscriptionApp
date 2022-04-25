using Amazon.DynamoDBv2;
using MusicSubscriptionApp.AWS;
using MusicSubscriptionApp.Models;

namespace MusicSubscriptionApp.Security
{
    public class Login
    {
        public static async Task<User> ValidateLoginCredentials(IAmazonDynamoDB client, string email, string password)
        {
            if (!(email == null) || !(password == null))
            {
                User user = await DynamoDB.GetUser(client, email);

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
