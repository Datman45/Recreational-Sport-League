using System.ComponentModel.DataAnnotations;

namespace Domain;

public class TeamStanding : BaseEntity{
    public Guid TeamId { get; set; }
    [Display(Name = "Team")]
    public string TeamName { get; set; } = string.Empty;
    public Guid SeasonId { get; set; }
    [Display(Name = "Season")]
    public string SeasonName { get; set; } = string.Empty;
    
    public int Wins { get; set; }
    public int Draws { get; set; }
    public int Losses { get; set; }
    [Display(Name = "Goals For")]
    public int GoalsFor { get; set; }
    [Display(Name = "Goals Against")]
    public int GoalsAgainst { get; set; }
    [Display(Name = "Goals Difference")]
    public int GoalsDifference => GoalsFor - GoalsAgainst;
    public int Points => Wins * 3 + Draws;
    [Display(Name = "Fair Play Points")]
    public int FairPlayPoints { get; set; }
}
