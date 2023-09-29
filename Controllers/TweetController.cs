using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TwitterCloneAPI.Data;
using TwitterCloneAPI.Exceptions; // Add the appropriate namespace for your custom exception
using TwitterCloneAPI.Models;

[ApiController]
[Route("api/tweet")]
[Authorize] // Ensure only authenticated users can access these endpoints
public class TweetController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public TweetController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Endpoint for creating a new tweet
    [HttpPost]
    public async Task<IActionResult> CreateTweet(Tweet tweet)
    {
        try
        {
            if (ModelState.IsValid)
            {
                // Check if a tweet with the same content already exists
                var existingTweet = await _context.Tweets
                    .FirstOrDefaultAsync(t => t.Content == tweet.Content);

                if (existingTweet != null)
                {
                    // A tweet with the same content already exists, so throw a custom exception
                    throw new DuplicateTweetException("A tweet with the same content already exists.");
                }

                tweet.Timestamp = DateTime.Now;
                _context.Tweets.Add(tweet);
                await _context.SaveChangesAsync();
                return Ok(new { Message = "Tweet created successfully" });
            }
            return BadRequest(ModelState);
        }
        catch (DuplicateTweetException ex)
        {
            // Handle the custom exception and return a response
            return BadRequest(new { Message = ex.Message });
        }
    }

    // Endpoint for liking a tweet
    [HttpPost("{id}/like")]
    public async Task<IActionResult> LikeTweet(int id)
    {
        var tweet = await _context.Tweets.FindAsync(id);

        if (tweet == null)
        {
            return NotFound();
        }

        // Implement your like logic here
        // Update tweet's like count, user's liked-tweets list, etc.
        // Save changes to the database

        return Ok(new { Message = "Tweet liked successfully" });
    }

    // Endpoint for commenting on a tweet
    [HttpPost("{id}/comment")]
    public async Task<IActionResult> CommentOnTweet(int id, string comment)
    {
        var tweet = await _context.Tweets.FindAsync(id);

        if (tweet == null)
        {
            return NotFound();
        }

        // Implement your comment logic here
        // Add the comment to the tweet's comments list, update timestamps, etc.
        // Save changes to the database

        return Ok(new { Message = "Comment added successfully" });
    }

    // Endpoint for sharing (retweeting) a tweet
    [HttpPost("{id}/share")]
    public async Task<IActionResult> ShareTweet(int id)
    {
        var tweet = await _context.Tweets.FindAsync(id);

        if (tweet == null)
        {
            return NotFound();
        }

        // Implement your share (retweet) logic here
        // Create a new tweet based on the original tweet, update timestamps, user, etc.
        // Save changes to the database

        return Ok(new { Message = "Tweet shared successfully" });
    }
}
