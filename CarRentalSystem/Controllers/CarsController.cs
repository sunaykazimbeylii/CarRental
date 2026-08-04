using CarRental.Application.DTOs.Car;
using CarRental.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CarsController : ControllerBase
{
    private readonly ICarService _service;

    public CarsController(ICarService service)
    {
        _service = service;
    }

    /// <summary>
    /// Returns a paginated list of cars with filtering and sorting options.
    /// </summary>
    /// <param name="filter">Car filter criteria.</param>
    /// <param name="page">Page number.</param>
    /// <param name="take">Number of items per page.</param>
    /// <param name="sort">Sort field.</param>
    /// <returns>List of cars.</returns>
    [Authorize(Roles = "User")]
    [HttpGet]
    public async Task<IActionResult> GetAll(
    [FromQuery] CarFilterDto filter,
    int page,
    int take,
    string? sort
      )
    {
        return Ok(await _service.GetAllAsync(filter,page, take, sort));
    }

    /// <summary>
    /// Returns a car by its identifier.
    /// </summary>
    /// <param name="id">Car identifier.</param>
    /// <returns>Car details.</returns>
    [Authorize(Roles = "User")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        return Ok(await _service.GetByIdAsync(id));
    }

    /// <summary>
    /// Creates a new car.
    /// </summary>
    /// <param name="dto">Car information.</param>
    /// <returns>Created response.</returns>
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody]CarCreateDto dto)
    {
        await _service.CreateAsync(dto);

        return StatusCode(StatusCodes.Status201Created);
    }
    /// <summary>
    /// Updates an existing car.
    /// </summary>
    /// <param name="id">Car identifier.</param>
    /// <param name="dto">Updated car information.</param>
    /// <returns>No content.</returns>
    [Authorize(Roles = "Admin")]

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] CarUpdateDto dto)
    {
        if (id < 1) return BadRequest();
        if (id != dto.Id)
            return BadRequest();
        

        await _service.UpdateAsync(dto);

        return NoContent();
    }

    /// <summary>
    /// Deletes a car by its identifier.
    /// </summary>
    /// <param name="id">Car identifier.</param>
    /// <returns>No content.</returns>
    [Authorize(Roles = "Admin")]

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        await _service.DeleteAsync(id);

        return NoContent();
    }
}