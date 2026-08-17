using Microsoft.AspNetCore.Http;

namespace CarRental.Application.Interfaces.Services;

public interface ICloudinaryService
{
    Task<string> UploadAsync(IFormFile file);
    Task<bool> DeleteAsync(string publicId);
}