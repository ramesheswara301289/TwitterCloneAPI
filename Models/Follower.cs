
using System;

namespace TwitterCloneAPI.Models
{
    public class Follower
    {
        public string FollowerId { get; set; }
        public ApplicationUser FollowerUser { get; set; }

        public string FolloweeId { get; set; }
        public ApplicationUser FolloweeUser { get; set; }
    }
}