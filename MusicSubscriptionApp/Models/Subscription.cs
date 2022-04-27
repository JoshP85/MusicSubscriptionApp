using Amazon.DynamoDBv2.DataModel;

namespace MusicSubscriptionApp.Models
{
    [DynamoDBTable("music")]
    public class Subscription
    {
        public Subscription(string artist, string title, string imgUrl, string webUrl, string year)
        {
            Artist = artist;
            Title = title;
            ImgUrl = imgUrl;
            WebUrl = webUrl;
            Year = year;
        }

        [DynamoDBHashKey]
        public string Artist { get; set; }

        [DynamoDBRangeKey]
        public string Title { get; set; }

        [DynamoDBProperty("img_url")]
        public string ImgUrl { get; set; }

        [DynamoDBProperty("web_url")]
        public string WebUrl { get; set; }

        [DynamoDBProperty]
        public string Year { get; set; }
    }
}
