namespace MoviesApi.Core.Dtos
{
    public class LoginUserDto
    {
        [MaxLength(50)]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [MinLength(8)]
        public string Password { get; set; } = null!;
    }
}
