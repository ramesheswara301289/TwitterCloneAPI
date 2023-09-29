using System.ComponentModel.DataAnnotations;

namespace TwitterCloneAPI.Models
{
    public class Tweet
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(280)]
        public string Text { get; set; }

        public string UserId { get; set; }

        public DateTime Timestamp { get; set; }
        public User User { get; set; }

        public string Content { get; set; } // Add this property

    }
}
