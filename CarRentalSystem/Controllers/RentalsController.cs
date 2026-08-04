using CarRental.Application.DTOs.Rental;
using CarRental.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RentalsController : ControllerBase
{
    private readonly IRentalService _service;

    public RentalsController(IRentalService service)
    {
        _service = service;
    }

    /// <summary>
    /// Returns all rentals.
    /// </summary>
    [Authorize(Roles = "ADMIN")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    /// <summary>
    /// Returns rental by id.
    /// </summary>
    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        return Ok(await _service.GetByIdAsync(id));
    }

    /// <summary>
    /// Creates a new rental.
    /// </summary>
    [Authorize(Roles = "USER")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RentalCreateDto dto)
    {
        await _service.CreateAsync(dto);

        return StatusCode(StatusCodes.Status201Created);
    }
}