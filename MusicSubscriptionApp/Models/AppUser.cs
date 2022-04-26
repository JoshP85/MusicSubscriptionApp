using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using System.ComponentModel.DataAnnotations;

namespace MusicSubscriptionApp.Models
{
    public class AppUser
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; }

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

        public static async Task<bool> CreateAppUser(IAmazonDynamoDB client, AppUser newUser)
        {
            if (newUser is null)
            {
                return false;
            }
            var requestSeed = new PutItemRequest
            {
                TableName = "login",
                Item = new Dictionary<string, AttributeValue>()
                    {
                        { "Email", new AttributeValue {S = newUser.Email.ToString() } },
                        { "User_Name", new AttributeValue {S = newUser.Username.ToString() } },
                        { "Password", new AttributeValue {S = newUser.Password.ToString() } },

                    }
            };
            await client.PutItemAsync(requestSeed);

            return true;
        }
    }
}
