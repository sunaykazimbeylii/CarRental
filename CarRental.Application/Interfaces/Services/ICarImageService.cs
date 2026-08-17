using Microsoft.AspNetCore.Http;

namespace CarRental.Application.Interfaces.Services;

public interface ICarImageService
{
    Task<string> UploadAsync(long carId, IFormFile file);

    Task<(byte[] File, string ContentType)> DownloadAsync(long imageId);
}