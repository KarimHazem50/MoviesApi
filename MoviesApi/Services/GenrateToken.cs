using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MoviesApi.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MoviesApi.Services
{
    public class GenrateToken : IGenrateToken
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtSetting _jwtSetting;

        public GenrateToken(UserManager<ApplicationUser> userManager, IOptions<JwtSetting> jwtSetting)
        {
            _userManager = userManager;
            _jwtSetting = jwtSetting.Value;
        }

        public async Task<AuthDto> CreateTokenAsync(ApplicationUser user)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.Key));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            var token = new JwtSecurityToken(
                    issuer: _jwtSetting.Issuer,
                    audience: _jwtSetting.Audience,
                    claims: claims,
                    expires: DateTime.Now.AddDays(_jwtSetting.DurationInDays),
                    signingCredentials: signingCredentials
                );

            return new AuthDto
            {
                IsAuthenticated = true,
                Email = user.Email,
                UserName = user.UserName,
                Roles = await _userManager.GetRolesAsync(user),
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpireOn = token.ValidTo,
            };
        }
    }
}
