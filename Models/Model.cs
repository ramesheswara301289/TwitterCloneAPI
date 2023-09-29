namespace TwitterCloneAPI.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }
        public string UserId { get; set; } // ID of the user who posted the comment
        public ApplicationUser User { get; set; } // Navigation property to the user
    }
}
