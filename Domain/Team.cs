using System.ComponentModel.DataAnnotations;

namespace Domain;

public class Team : BaseEntity {
    public string Name { get; set; } = string.Empty;
    [Display(Name = "Lights")]
    public bool HasLights { get; set; }
    [Display(Name = "Home Field")]
    public string HomeField { get; set; } = string.Empty;
}
