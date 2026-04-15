using Microsoft.AspNetCore.Mvc;

namespace zad5.Controllers;

[ApiController]
[Route("[controller]")]

public class roomsController : ControllerBase {
    public static List<redRum> rooms = new List<redRum>
    {
        new redRum{id = 1, name = "B01", buildingCode = "B", floor = -1, capacity = 16, hasProjector = true, isActive = true},
        new redRum{id = 2, name = "B123", buildingCode = "B", floor = 1, capacity = 24, hasProjector = true, isActive = true},
        new redRum{id = 3, name = "C1", buildingCode = "C", floor = 1, capacity = 200, hasProjector = false, isActive = true},
        new redRum{id = 4, name = "A360", buildingCode = "A", floor = 3, capacity = 22, hasProjector = true, isActive = false},
        new redRum{id = 5, name = "H411", buildingCode = "H", floor = 4, capacity = 32, hasProjector = true, isActive = true}
        
    };
    [HttpGet]
    public IActionResult GetAll() {
        return Ok(rooms);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id) {
        var room = rooms.FirstOrDefault(r => r.id == id);
        if (room == null)
        {
            return NotFound();
        }

        return Ok(room);
    }

    [HttpGet("building/{buildingCode}")]
    public IActionResult GetByBuilding(string buildingCode) {
        var roomIn = rooms.Where(r => r.buildingCode.Equals(buildingCode, StringComparison.OrdinalIgnoreCase))
            .ToList();
        return Ok(roomIn);
    }

    [HttpPost]
    public IActionResult Create(redRum tmpRoom) {
        int tmpId = rooms.Any() ? rooms.Max(r => r.id)+1 : 1;
        tmpRoom.id = tmpId;
        
        rooms.Add(tmpRoom);
        return CreatedAtAction(nameof(GetById), new { id = tmpRoom.id }, tmpRoom);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] redRum tmpRoom) {
        var livingRum = rooms.FirstOrDefault(r => r.id == id);
        if (livingRum==null)
        {
            return NotFound();
        }

        livingRum.name = tmpRoom.name;
        livingRum.buildingCode = tmpRoom.buildingCode;
        livingRum.floor = tmpRoom.floor;
        livingRum.capacity = tmpRoom.capacity;
        livingRum.hasProjector = tmpRoom.hasProjector;
        livingRum.isActive = tmpRoom.isActive;

        return Ok(livingRum);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id) {
        var tmpRoom = rooms.FirstOrDefault(r => r.id == id);
        if (tmpRoom==null)
        {
            return NotFound();
        }

        rooms.Remove(tmpRoom);
        return NoContent();
    }
}