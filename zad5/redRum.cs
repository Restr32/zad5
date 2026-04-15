using System.ComponentModel.DataAnnotations;
using Microsoft.VisualBasic.CompilerServices;

namespace zad5;

public class redRum {
    public int id { get; set; }
    [Required(ErrorMessage = "Wymagana nazwa")]
    public string name { get; set; }
    [Required(ErrorMessage = "Wymagany jest kod budynku")]
    public string buildingCode { get; set; }
    public int floor { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Pojemość musi być większa od zera")]
    public int capacity { get; set; }
    public bool hasProjector { get; set; }
    public bool isActive { get; set; }
    
    public redRum(){}
}