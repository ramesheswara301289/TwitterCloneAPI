using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TwitterCloneAPI.Data;
using TwitterCloneAPI.Exceptions;
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

        // Check if the user has already liked the tweet
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Get the user's ID

        if (tweet.LikedByUsers == null)
        {
            tweet.LikedByUsers = new List<string>(); // Initialize the collection if it's null
        }

        if (tweet.LikedByUsers.Contains(userId))
        {
            // User has already liked the tweet, so remove the like
            tweet.LikeCount--;
            tweet.LikedByUsers.Remove(userId);
        }
        else
        {
            // User has not liked the tweet, so add the like
            tweet.LikeCount++;
            tweet.LikedByUsers.Add(userId);
        }

        // Save changes to the database
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Tweet liked successfully" });
    }

    // Endpoint for commenting on a tweet
    [HttpPost("{id}/comment")]
    public async Task<IActionResult> CommentOnTweet(int id, [FromBody] string commentText)
    {
        var tweet = await _context.Tweets.FindAsync(id);

        if (tweet == null)
        {
            return NotFound();
        }

        if (string.IsNullOrWhiteSpace(commentText))
        {
            return BadRequest("Comment text cannot be empty.");
        }

        var comment = new Comment
        {
            Text = commentText,
            Timestamp = DateTime.Now,
            UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
        };

        tweet.Comments.Add(comment);
        tweet.Timestamp = DateTime.Now;

        await _context.SaveChangesAsync();

        return Ok(new { Message = "Comment added successfully" });
    }



    // Endpoint for sharing (retweeting) a tweet
    [HttpPost("{id}/share")]
    public async Task<IActionResult> ShareTweet(int id)
    {
        var tweetToShare = await _context.Tweets.FindAsync(id);

        if (tweetToShare == null)
        {
            return NotFound();
        }

        // Create a new tweet based on the original tweet
        var retweet = new Tweet
        {
            Content = tweetToShare.Content, // You may modify this if you want to add a prefix like "Retweet: "
            Timestamp = DateTime.Now,
            UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
        };

        // Add the new retweet to the context
        _context.Tweets.Add(retweet);

        // Save changes to the database
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Tweet shared successfully" });
    }
}
