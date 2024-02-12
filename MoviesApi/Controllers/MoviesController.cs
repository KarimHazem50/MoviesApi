using Microsoft.AspNetCore.Authorization;

namespace MoviesApi.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IImageServices _imageServices;
        public MoviesController(ApplicationDbContext context, IImageServices imageServices)
        {
            _context = context;
            _imageServices = imageServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var movies = await _context.Movies.OrderByDescending(m => m.Rate).Include(m => m.Genre).ToListAsync();
            return Ok(movies);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var movie = await _context.Movies.Include(m => m.Genre).SingleOrDefaultAsync(m => m.Id == id);
            if (movie == null)
                return NotFound($"No movie was found with ID:{id}");

            return Ok(movie);
        }


        [HttpGet]
        [Route("GetMovies/{genrelid}")]
        public async Task<IActionResult> GetMovis(byte genrelid)
        {
            var genre = _context.Genres.Find(genrelid);
            if (genre is null)
                return NotFound($"No genre was found with ID: {genrelid}");

            var movies = await _context.Movies.Include(m => m.Genre).Where(m => m.GenreId == genrelid).ToListAsync();

            return Ok(movies);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMovieAsync([FromForm] MovieDto dto)
        {
            var isValidGenre = await _context.Genres.AnyAsync(g => g.Id == dto.GenreId);
            if (!isValidGenre)
                return BadRequest("Invalid Genre Id");

            var movie = new Movie
            {
                Title = dto.Title,
                Year = dto.Year,
                Rate = dto.Rate,
                StoryLine = dto.StoryLine,
                GenreId = dto.GenreId,
            };

            if (dto.Image != null)
            {
                var extension = Path.GetExtension(dto.Image.FileName);
                var imageName = $"{Guid.NewGuid()}{extension}";
                var result = await _imageServices.UploadImageAsync(dto.Image, imageName, "/Images/Movies");
                if (!result.isUploaded)
                {
                    return BadRequest(result.errorMessage);
                }
                movie.ImageUrl = imageName;
            }
            await _context.AddAsync(movie);
            _context.SaveChanges();
            return Ok(movie);
        }


        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateMovieAsync(int id, [FromForm] MovieDto dto)
        {
            var movie = await _context.Movies.SingleOrDefaultAsync(m => m.Id == id);
            if (movie is null)
                return NotFound($"No genre was found with ID: {id}");

            var isValidGenre = await _context.Genres.AnyAsync(g => g.Id == dto.GenreId);
            if (!isValidGenre)
                return BadRequest("Invalid Genre Id");

            movie.Title = dto.Title;
            movie.Year = dto.Year;
            movie.Rate = dto.Rate;
            movie.StoryLine = dto.StoryLine;
            movie.GenreId = dto.GenreId;

            if (movie.ImageUrl != null)
                _imageServices.Delete(FolderPath: "/Images/Movies/", ImageName: movie.ImageUrl);
            movie.ImageUrl = null;

            if (dto.Image != null)
            {
                var extension = Path.GetExtension(dto.Image.FileName);
                var imageName = $"{Guid.NewGuid()}{extension}";
                var result = await _imageServices.UploadImageAsync(dto.Image, imageName, "/Images/Movies/");

                if (!result.isUploaded)
                    return BadRequest(result.errorMessage);

                movie.ImageUrl = imageName;
            }
            _context.SaveChanges();
            return Ok(movie);
        }



        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteMovieAsync(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie is null)
                return NotFound($"No Movie was found with ID: {id}");

            if (movie.ImageUrl != null)
                _imageServices.Delete(FolderPath: "/Images/Movies/", ImageName: movie.ImageUrl);
            try
            {
                _context.Movies.Remove(movie);
                _context.SaveChanges();
                return Ok(movie);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}