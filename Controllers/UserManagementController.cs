using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using TwitterCloneAPI.Data;
using TwitterCloneAPI.Models;

[ApiController]
[Route("api/user-management")]
[Authorize]
public class UserManagementController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public UserManagementController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("profile")]
    public async Task<IActionResult> GetUserProfile()
    {
        // Retrieve the user's profile
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(up => up.UserId == userId);

        if (userProfile == null)
        {
            return NotFound(); // Profile not found
        }

        return Ok(userProfile);
    }

    [HttpPut("profile")]
    public async Task<IActionResult> UpdateUserProfile(UserProfile updatedProfile)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(up => up.UserId == userId);

        if (userProfile == null)
        {
            return NotFound();
        }

        // Update profile properties
        userProfile.Name = updatedProfile.Name;
        userProfile.ProfilePictureUrl = updatedProfile.ProfilePictureUrl;
        userProfile.Bio = updatedProfile.Bio;

        try
        {
            await _context.SaveChangesAsync();
            return Ok(new { Message = "Profile updated successfully" });
        }
        catch (DbUpdateException)
        {
            // Handle database update error
            return StatusCode(500, "An error occurred while updating the profile.");
        }
    }
}
