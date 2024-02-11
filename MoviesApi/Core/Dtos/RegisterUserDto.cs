namespace MoviesApi.Core.Dtos
{
    public class RegisterUserDto
    {
        [MaxLength(30)]
        public string UserName { get; set; } = null!;

        [MaxLength(50)]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [MinLength(8)]
        public string Password { get; set; } = null!;

        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = null!;
    }
}
