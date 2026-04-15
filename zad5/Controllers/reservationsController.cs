using Microsoft.AspNetCore.Mvc;

namespace zad5.Controllers;

[ApiController]
[Route("[controller]")]

public class reservationsController : ControllerBase {
    public static List<Reservation> reserv = new List<Reservation>();

    [HttpGet]
    public IActionResult GetAll() {
        return Ok(reserv);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id) {
        var tmpRes = reserv.FirstOrDefault(r => r.id == id);
        return tmpRes == null ? NotFound() : Ok(tmpRes);
    }

    [HttpGet]
    public IActionResult GetFiltered([FromQuery] DateOnly? date, [FromQuery] string? status, [FromQuery] int? id) {
        var all = reserv.AsQueryable();

        if (date.HasValue)
        {
            all = all.Where(r => r.date == date.Value);
        }

        if (!string.IsNullOrEmpty(status))
        {
            all = all.Where(r => r.status.Equals(status, StringComparison.OrdinalIgnoreCase));
        }

        if (id.HasValue)
        {
            all = all.Where(r => r.roomId == id.Value);
        }

        return Ok(all.ToList());
    }
}