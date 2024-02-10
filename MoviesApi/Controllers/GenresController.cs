namespace MoviesApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public GenresController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var genres = await _context.Genres.OrderBy(g => g.Name).ToListAsync();

            return Ok(genres);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetByIdAsync(byte id)
        {
            var genre = await _context.Genres.FindAsync(id);

            return Ok(genre);
        }

        [HttpPost]
        public async Task<IActionResult> AddGenreAsync(GenreDto dto)
        {
            var genre = new Genre
            {
                Name = dto.Name
            };

            await _context.Genres.AddAsync(genre);
            _context.SaveChanges();

            return Ok(genre);
        }

        [HttpPost]
        [Route("AddGenres")]
        public async Task<IActionResult> AddGenresAsync(List<GenreDto> dtos)
        {
            var genres = new List<Genre>();
            foreach (var dto in dtos)
            {
                var genre = new Genre
                {
                    Name = dto.Name
                };
                genres.Add(genre);
            }
            await _context.Genres.AddRangeAsync(genres);
            _context.SaveChanges();

            return Ok(genres);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateGenreAsync(byte id, [FromBody] GenreDto dto)
        {
            var genre = await _context.Genres.FindAsync(id);
            if (genre is null)
                return NotFound($"No genre was found with ID:{id}");

            genre.Name = dto.Name;

            await _context.SaveChangesAsync();
            return Ok(genre);
        }


        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteGenreAsync(byte id)
        {
            var genre = await _context.Genres.FindAsync(id);
            if (genre is null)
                return NotFound($"No genre was found with ID:{id}");

            try
            {
                _context.Genres.Remove(genre);
                _context.SaveChanges();
                return Ok("Removed Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
