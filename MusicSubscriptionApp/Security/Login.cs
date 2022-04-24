using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using MusicSubscriptionApp.Models;

namespace MusicSubscriptionApp.Security
{
    public class Login
    {
        public static async Task<User> ValidateLoginCredentials(IAmazonDynamoDB client, string email, string password)
        {
            if (!(email == null) || !(password == null))
            {
                Table tableName = Table.LoadTable(client, "login");

                Document UserDocument = await tableName.GetItemAsync(email);

                if (UserDocument != null)
                {
                    User user = new()
                    {
                        Email = UserDocument["Email"],
                        Username = UserDocument["User_Name"],
                        Password = UserDocument["Password"],
                    };

                    if (user.Password == password)
                        return user;
                }
            }
            return null;
        }
    }
}
