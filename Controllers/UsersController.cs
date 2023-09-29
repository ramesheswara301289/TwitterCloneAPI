using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TwitterCloneAPI.Models; // Adjust the namespace as needed
//using TwitterCloneAPI.ViewModels; // You may need to create this ViewModel

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
                Email = model.Email // You can add additional user properties here
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // You can customize the response on successful registration
                return Ok(new { Message = "Registration successful" });
            }

            // Handle registration errors
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        // Return validation errors
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
