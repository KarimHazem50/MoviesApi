namespace MoviesApi.Core.Models
{
    public class Genre
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = null!;

        //public IList<Movie> Movies { get; set; } = new List<Movie>();
    }
}
