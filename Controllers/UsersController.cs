using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TwitterCloneAPI.Models;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                BirthDate = model.BirthDate,
                ProfilePictureUrl = model.ProfilePictureUrl,
                Bio = model.Bio
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Registration succeeded, customize the response here
                var customResponse = new
                {
                    Message = "Registration successful",
                    UserId = user.Id,
                    email = user.Email
                };
                return Ok(customResponse);
            }
            else
            {
                // Registration failed, handle and report errors
                var errors = result.Errors.Select(error => error.Description);

                var errorResponse = new
                {
                    Message = "Registration failed",
                    Errors = errors
                };

                return BadRequest(errorResponse);
            }
        }
            return BadRequest(ModelState);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                // You can customize the response on successful login
                return Ok(new { Message = "Login successful" });
            }

            if (result.IsLockedOut)
            {
                // Handle account lockout
                return BadRequest(new { Message = "Account locked out" });
            }
        }

        // Handle login failures
        ModelState.AddModelError(string.Empty, "Invalid login attempt");
        return BadRequest(ModelState);
    }
}
