using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using MusicSubscriptionApp.Models;

namespace MusicSubscriptionApp.Services
{
    public class AppUserControllerServices
    {
        public static async Task<AppUser> GetAppUser(IAmazonDynamoDB client, string email)
        {
            if (email != null)
            {
                Table tableName = Table.LoadTable(client, "login");

                Document UserDocument = await tableName.GetItemAsync(email);

                if (UserDocument != null)
                {
                    AppUser user = new()
                    {
                        Email = UserDocument["Email"],
                        Username = UserDocument["User_Name"],
                        Password = UserDocument["Password"],
                    };
                    return user;
                }
            }
            return null;
        }
    }
}
