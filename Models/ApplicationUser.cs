using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

public class ApplicationUser : IdentityUser
{
    // User properties
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string ProfilePictureUrl { get; set; }
    public string Bio { get; set; }
    public DateTime BirthDate { get; set; }

    // Navigation property for followers
    public ICollection<ApplicationUser> Followers { get; set; }

    // Navigation property for users the current user is following
    public ICollection<ApplicationUser> Following { get; set; }

    // Additional properties can be added here

    // Navigation properties
    // For example, if a user has tweets, you can define a navigation property like this:
    // public ICollection<Tweet> Tweets { get; set; }
}
