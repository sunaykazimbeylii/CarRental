using CarRental.Application.Exceptions;
using CarRental.Application.Interfaces.Repositories;
using CarRental.Application.Interfaces.Repository.Generic;
using CarRental.Application.Interfaces.Services;
using CarRental.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace CarRental.Persistence.Services;

public class CarImageService : ICarImageService
{
    private readonly ICloudinaryService _cloudinaryService;
    private readonly ICarRepository _carRepository;
    private readonly IRepository<CarImage> _imageRepository;

    private const long MaxFileSize = 5 * 1024 * 1024;

    private static readonly string[] AllowedExtensions =
    {
        ".jpg",
        ".jpeg",
        ".png",
        ".webp"
    };

    public CarImageService(
        ICloudinaryService cloudinaryService,
        ICarRepository carRepository,
        IRepository<CarImage> imageRepository)
    {
        _cloudinaryService = cloudinaryService;
        _carRepository = carRepository;
        _imageRepository = imageRepository;
    }

    public async Task<string> UploadAsync(
        long carId,
        IFormFile file)
    {
       
        if (file == null || file.Length == 0)
            throw new ArgumentException("Fayl boş ola bilməz.");

     
        if (file.Length > MaxFileSize)
            throw new ArgumentException(
                "Faylın ölçüsü maksimum 5 MB ola bilər.");

   
        var extension =
            Path.GetExtension(file.FileName)
                .ToLowerInvariant();

        if (!AllowedExtensions.Contains(extension))
            throw new ArgumentException(
                "Yalnız JPG, JPEG, PNG və WEBP fayllarına icazə verilir.");

     
        var car = await _carRepository.GetByIdAsync(carId);

        if (car == null)
            throw new NotFoundException(nameof(Car));

      
        var imageUrl =
            await _cloudinaryService.UploadAsync(file);

    
        var image = new CarImage
        {
            ImageUrl = imageUrl,
            CarId = carId
        };

        _imageRepository.Add(image);

        await _imageRepository.SaveChangesAsync();

        return imageUrl;
    }

    public async Task<(byte[] File, string ContentType)> DownloadAsync(
        long imageId)
    {
        var image = _imageRepository
            .GetAll()
            .FirstOrDefault(x => x.Id == imageId);

        if (image == null)
            throw new NotFoundException(nameof(CarImage));

        using var httpClient = new HttpClient();

        var response =
            await httpClient.GetAsync(image.ImageUrl);

        if (!response.IsSuccessStatusCode)
            throw new FileNotFoundException(
                "Fayl Cloudinary-də tapılmadı.");

        var fileBytes =
            await response.Content.ReadAsByteArrayAsync();

        var contentType =
            response.Content.Headers.ContentType?.MediaType
            ?? "application/octet-stream";

        return (fileBytes, contentType);
    }
}