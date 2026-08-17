using CarRental.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarImagesController : ControllerBase
{
    private readonly ICarImageService _carImageService;

    public CarImagesController(ICarImageService carImageService)
    {
        _carImageService = carImageService;
    }

    [HttpPost("{carId}")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Upload(
        long carId,
        IFormFile file)
    {
        var imageUrl = await _carImageService.UploadAsync(
            carId,
            file);

        return Ok(new
        {
            message = "Şəkil uğurla yükləndi.",
            imageUrl
        });
    }

    [HttpGet("{imageId}")]
    public async Task<IActionResult> Download(long imageId)
    {
        var result = await _carImageService.DownloadAsync(imageId);

        return File(
            result.File,
            result.ContentType);
    }
}