using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.S3;
using Newtonsoft.Json;

namespace MusicSubscriptionApp.Data
{
    public class SeedData
    {
        public static async Task SeedMusicTable(IAmazonDynamoDB client, IAmazonS3 clientS3)
        {
            var text = File.ReadAllText(@"a2.json");
            dynamic? data = JsonConvert.DeserializeObject(text);

            if (data is null)
            {
                return;
            }

            foreach (var item in data.songs)
            {
                string imgurl = item.img_url.ToString();
                string songID = Guid.NewGuid().ToString();

                await S3ImageUpload.UploadToS3(imgurl, songID, clientS3);

                var requestSeed = new PutItemRequest
                {
                    TableName = "music",
                    Item = new Dictionary<string, AttributeValue>()
                    {
                        { "Artist", new AttributeValue {S = item.artist } },
                        { "Title", new AttributeValue {S = item.title } },
                        { "Year", new AttributeValue {S = item.year } },
                        { "web_url", new AttributeValue {S = item.web_url } },
                        { "img_url", new AttributeValue {S = item.img_url } },
                        { "SongID", new AttributeValue {S = songID}},
                    }
                };
                await client.PutItemAsync(requestSeed);
            }
            return;
        }

        public static async Task SeedLoginTable(IAmazonDynamoDB client)
        {
            var text = File.ReadAllText(@"login.json");
            dynamic? data = JsonConvert.DeserializeObject(text);

            if (data is null)
            {
                return;
            }

            foreach (var item in data.logins)
            {
                var requestSeed = new PutItemRequest
                {
                    TableName = "login",
                    Item = new Dictionary<string, AttributeValue>()
                    {
                        { "Email", new AttributeValue {S = item.email } },
                        { "User_Name", new AttributeValue {S = item.user_name } },
                        { "Password", new AttributeValue {S = item.password } },
                    }

                };
                await client.PutItemAsync(requestSeed);
            }
            return;
        }
    }
}
