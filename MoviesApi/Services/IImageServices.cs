namespace MoviesApi.Services
{
    public interface IImageServices
    {
       Task<(bool isUploaded, string? errorMessage)> UploadImageAsync(IFormFile Image, string ImageName, string FolderPath);

        void Delete(string ImageName, string FolderPath);

    }
}
