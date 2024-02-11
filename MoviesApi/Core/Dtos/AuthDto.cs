namespace MoviesApi.Core.Dtos
{
    public class AuthDto
    {
        public bool IsAuthenticated { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public IList<string>? Roles { get; set; }
        public string? Token { get; set; }
        public DateTime? ExpireOn { get; set; }

    }
}
