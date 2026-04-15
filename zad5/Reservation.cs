using System.ComponentModel.DataAnnotations;

namespace zad5;

public class Reservation {
    public int id { get; set; }
    [Required]
    public int roomId { get; set; }
    [Required(ErrorMessage = "Wymagany organizator")]
    public string organizerName { get; set; }
    [Required]
    public string topic { get; set; }
    public DateOnly date { get; set; }
    public TimeOnly startTime { get; set; }
    public TimeOnly endTime { get; set; }
    [Required]
    public string status { get; set; }
    
    public Reservation(){}
}