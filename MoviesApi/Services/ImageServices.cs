namespace MoviesApi.Services
{
    public class ImageServices : IImageServices
    {
        private readonly List<string> _allowedExtension = new() { ".png", ".jpeg", ".jpg" };
        private readonly int _allowedMaxSize = 2097152;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ImageServices(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<(bool isUploaded, string? errorMessage)> UploadImageAsync(IFormFile Image, string ImageName, string FolderPath)
        {
            var extension = Path.GetExtension(Image.FileName);
            if (!_allowedExtension.Contains(extension))
                return (isUploaded: false, errorMessage: Errors.ExtensionNotAllowed);

            if (Image.Length > _allowedMaxSize)
                return (isUploaded: false, errorMessage: Errors.SizeNotAllowed);

            var path = Path.Combine($"{_webHostEnvironment.WebRootPath}{FolderPath}", ImageName);
            using var stream = File.Create(path);
            await Image.CopyToAsync(stream);
            await stream.DisposeAsync();

            return (isUploaded: true, errorMessage: null);
        }

        public void Delete(string ImageName, string FolderPath)
        {
            var path = Path.Combine($"{_webHostEnvironment.WebRootPath}{FolderPath}", ImageName);
            if(File.Exists(path))
                File.Delete(path);
        }

    }
}
