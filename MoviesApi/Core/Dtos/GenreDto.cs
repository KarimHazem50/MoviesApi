namespace MoviesApi.Core.Dtos
{
    public class GenreDto
    {
        [MaxLength(100)]
        public string Name { get; set; } = null!;
    }
}
