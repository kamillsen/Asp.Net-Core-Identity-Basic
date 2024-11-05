using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using User.Managment.API.Models.Authentication.SignUp;

namespace User.Managment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthenticationController(UserManager<IdentityUser> userManager,
                                        RoleManager<IdentityRole> roleManager,
                                        IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterUser registerUser, string role)
        {
            // Check User Exist 
            var userExist = await _userManager.FindByEmailAsync(registerUser.Email);    
            if(userExist != null)
            {
                return StatusCode(StatusCode.Status403Forbidden,
                    new ResponseCacheAttribute { Status = "Error", Message = "User already exists!"});
            }

            IdentityUser user = new()
            {
                Email = registerUser.Email,
                SecurityStamp = Guid.NewGuid().ToString(),  
                UserName = registerUser.Username

            };
            var result = await _userManager.CreateAsync(user, registerUser.Password);
            if(result.Succeeded)
            {
                return StatusCode(StatusCode.Status201Created,
                    new ResponseCacheAttribute { Status = "Success", Message = "User Created Success!" });
            }
            else
            {
                return StatusCode(StatusCode.Status500InternalServerError,
                    new ResponseCacheAttribute { Status = "Success", Message = "User Failed to Created!" });
            }
        }
    }

}
