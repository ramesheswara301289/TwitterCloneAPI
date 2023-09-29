using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TwitterCloneAPI.Data;
using TwitterCloneAPI.Models;

namespace TwitterCloneAPI.Controllers
{
    [ApiController]
    [Route("api/feed")]
    public class FeedController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FeedController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserFeed(string userId, int page = 1, int pageSize = 10)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                {
                    return NotFound();
                }

                // Get the user's followers
                var userFollowers = await _context.Followers
                .Where(f => f.FolloweeId == userId)
                .Select(f => f.FollowerId)
                .ToListAsync();

                // Include the user's own tweets and the tweets of their followers
                var feedTweets = _context.Tweets
                    .Where(tweet => tweet.UserId == userId || userFollowers.Contains(tweet.UserId))
                    .OrderByDescending(tweet => tweet.Timestamp)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return Ok(feedTweets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}