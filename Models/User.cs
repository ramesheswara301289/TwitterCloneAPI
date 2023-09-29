namespace TwitterCloneAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        // Add other user properties as needed
        public ICollection<Tweet> Tweets { get; set; }
    }
}
