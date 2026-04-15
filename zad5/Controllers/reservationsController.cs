using Microsoft.AspNetCore.Mvc;

namespace zad5.Controllers;

[ApiController]
[Route("[controller]")]

public class reservationsController : ControllerBase {
    public static List<Reservation> reserv = new List<Reservation>
    {
        new Reservation
        {
            id = 1, roomId = 1, organizerName = "dr inż. Kowalski", topic = "PPJ",
            date = new DateOnly(2026, 05, 10), startTime = new TimeOnly(08, 00),
            endTime = new TimeOnly(10, 00), status = "confirmed"
        },
        new Reservation
        {
            id = 2, roomId = 2, organizerName = "mgr Nowak", topic = "SWB",
            date = new DateOnly(2026, 05, 10), startTime = new TimeOnly(11, 00),
            endTime = new TimeOnly(13, 00), status = "planned"
        },
        new Reservation
        {
            id = 3, roomId = 3, organizerName = "Prof. Wiśniewski", topic = "ABPD",
            date = new DateOnly(2026, 05, 11), startTime = new TimeOnly(09, 00),
            endTime = new TimeOnly(11, 00), status = "confirmed"
        },
        new Reservation
        {
            id = 4, roomId = 5, organizerName = "dr Malinowski", topic = "TPO",
            date = new DateOnly(2026, 05, 12), startTime = new TimeOnly(10, 00),
            endTime = new TimeOnly(12, 00), status = "confirmed"
        },
        new Reservation
        {
            id = 5, roomId = 1, organizerName = "mgr Adamiak", topic = "JJP",
            date = new DateOnly(2026, 05, 12), startTime = new TimeOnly(14, 00),
            endTime = new TimeOnly(16, 00), status = "planned"
        },
        new Reservation
        {
            id = 6, roomId = 4, organizerName = "dr Zieliński", topic = "RBD",
            date = new DateOnly(2026, 05, 13), startTime = new TimeOnly(08, 00),
            endTime = new TimeOnly(10, 00), status = "confirmed"
        }
    };

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