using System.ComponentModel.DataAnnotations;

namespace Domain;

public class Season : BaseEntity{
    public string Name { get; set; } = string.Empty;
    [Display(Name = "Start Date")]
    public DateTime StartDate { get; set; }
    [Display(Name = "End Date")]
    public DateTime EndDate { get; set; }
    public int Year { get; set; }
}
