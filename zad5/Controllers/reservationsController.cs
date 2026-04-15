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

    [HttpPost]
    public IActionResult Create([FromBody] Reservation tmpRes) {
        if (tmpRes.endTime <= tmpRes.startTime)
        {
            return BadRequest("Godzina zakończenia jest wcześniejsza od startu");
        }

        var room = roomsController.rooms.FirstOrDefault(r => r.id == tmpRes.roomId);

        if (room == null)
        {
            return NotFound("Nie ma takiej klasy");
        }

        if (!room.isActive)
        {
            return BadRequest("Pomieszczenie nie jest w użytku");
        }

        bool conflict = reserv.Any(r =>
            r.roomId == tmpRes.roomId &&
            r.date == tmpRes.date &&
            r.status != "cancelled" &&
            !(tmpRes.endTime <= r.startTime || tmpRes.startTime >= r.endTime));
        if (conflict)
        {
            return Conflict("Dana sala jest już zajęta");
        }

        tmpRes.id = reserv.Any() ? reserv.Max(r => r.id) + 1 : 1;
        reserv.Add(tmpRes);

        return CreatedAtAction(nameof(GetById), new { id = tmpRes.id }, tmpRes);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] Reservation tmpRes) {
        var livingRes = reserv.FirstOrDefault(r => r.id == id);
        if (livingRes==null)
        {
            return NotFound();
        }

        if (tmpRes.endTime <= tmpRes.startTime)
        {
            return BadRequest("Godzina zakończenia jest wcześniejsza od startu");
        }

        livingRes.roomId = tmpRes.roomId;
        livingRes.organizerName = tmpRes.organizerName;
        livingRes.topic = tmpRes.topic;
        livingRes.date = tmpRes.date;
        livingRes.startTime = tmpRes.startTime;
        livingRes.endTime = tmpRes.endTime;
        livingRes.status = tmpRes.status;

        return Ok(livingRes);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id) {
        var resRemo = reserv.FirstOrDefault(r => r.id == id);
        if (resRemo==null)
        {
            return NotFound();
        }

        reserv.Remove(resRemo);
        return NoContent();
    }
}