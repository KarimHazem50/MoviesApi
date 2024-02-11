namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGenrateToken _genrateToken;

        public AccountController(UserManager<ApplicationUser> userManager, IGenrateToken genrateToken)
        {
            _userManager = userManager;
            _genrateToken = genrateToken;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegistrationAsync(RegisterUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new ApplicationUser
            {
                UserName = dto.UserName,
                Email = dto.Email,
            };
            

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return BadRequest(string.Join(", ", result.Errors.Select(e => e.Description)));

            await _userManager.AddToRoleAsync(user, Roles.User);

            // Create Token
            var authDto = await _genrateToken.CreateTokenAsync(user);

            return Ok(authDto);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync(LoginUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user is null)
                return Unauthorized();

            var found = await _userManager.CheckPasswordAsync(user, dto.Password);
            if(!found)
                return Unauthorized();

            // Create Token
            var authDto = await _genrateToken.CreateTokenAsync(user);

            return Ok(authDto);
        }
    }
}
