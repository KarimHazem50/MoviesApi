namespace MoviesApi.Services
{
    public interface IGenrateToken
    {
        Task<AuthDto> CreateTokenAsync(ApplicationUser user);
    }
}
