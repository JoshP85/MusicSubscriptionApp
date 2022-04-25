using System.ComponentModel.DataAnnotations;
namespace MusicSubscriptionApp.Models
{
    public class User
    {
        /*        public User()
                {
                }

                public User(string email, string username, string password)
                {
                    Email = email;
                    Username = username;
                    Password = password;
                }*/

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; }

    }
}
