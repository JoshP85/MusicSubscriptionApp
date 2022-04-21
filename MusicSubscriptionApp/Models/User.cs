namespace MusicSubscriptionApp.Models
{
    public class User
    {
        public User(string email, string username, string password, ICollection<Subscription> subscriptions) : this(email, username, password)
        {
            Subscriptions = subscriptions;
        }

        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; }

    }
}
