using Amazon.DynamoDBv2.DataModel;

namespace MusicSubscriptionApp.Models
{
    [DynamoDBTable("music")]
    public class Song
    {
        [DynamoDBHashKey]
        public string SongID { get; set; }

        [DynamoDBProperty]
        public string Artist { get; set; }

        [DynamoDBProperty]
        public string Title { get; set; }

        [DynamoDBProperty("img_url")]
        public string ImgUrl { get; set; }

        [DynamoDBProperty("web_url")]
        public string WebUrl { get; set; }

        [DynamoDBProperty]
        public string Year { get; set; }


    }
}
