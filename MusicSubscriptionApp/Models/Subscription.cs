namespace MusicSubscriptionApp.Models
{
    public class Subscription
    {
        public Subscription(string title, string artist, string year, string webUrl, string imgUrl)
        {
            Title = title;
            Artist = artist;
            Year = year;
            WebUrl = webUrl;
            ImgUrl = imgUrl;
        }

        public string Title { get; set; }
        public string Artist { get; set; }
        public string Year { get; set; }
        public string WebUrl { get; set; }
        public string ImgUrl { get; set; }

    }
}
