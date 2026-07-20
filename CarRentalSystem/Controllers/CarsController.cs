using CarRental.Application.DTOs.Car;
using CarRental.Application.Interfaces.Services;
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

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id}")]

    public async Task<IActionResult> GetById(long id)
    {
        return Ok(await _service.GetByIdAsync(id));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm]CarCreateDto dto)
    {
        await _service.CreateAsync(dto);

        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id,[FromForm] CarUpdateDto dto)
    {
        if (id < 1) return BadRequest();
        if (id != dto.Id)
            return BadRequest();
        

        await _service.UpdateAsync(dto);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        await _service.DeleteAsync(id);

        return NoContent();
    }
}