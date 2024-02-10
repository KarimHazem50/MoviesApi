namespace MoviesApi.Core.Dtos
{
    public class MovieDto
    {
        [MaxLength(250)]
        public string Title { get; set; } = null!;

        public int Year { get; set; }
        public double Rate { get; set; }

        [MaxLength(2500)]
        public string StoryLine { get; set; } = null!;

        public IFormFile? Image { get; set; }

        public byte GenreId { get; set; }
    }
}
