using Microsoft.AspNetCore.Mvc;

namespace zad5.Controllers;

[ApiController]
[Route("[controller]")]

public class SalController : ControllerBase {
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
    
    
}