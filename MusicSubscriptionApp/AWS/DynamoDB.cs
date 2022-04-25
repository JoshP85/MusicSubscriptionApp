using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using MusicSubscriptionApp.Models;

namespace MusicSubscriptionApp.AWS
{
    public class DynamoDB
    {

        public static async Task<User> GetUser(IAmazonDynamoDB client, string email)
        {
            if (email != null)
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
                    return user;
                }
            }
            return null;
        }
    }
}
