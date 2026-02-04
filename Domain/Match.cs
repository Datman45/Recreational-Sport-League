using System.ComponentModel.DataAnnotations;

namespace Domain;

public class Match : BaseEntity{
    public Guid HomeTeamId { get; set; }
    public Guid AwayTeamId { get; set; }
    public Guid SeasonId { get; set; }
    public Guid RefereeId { get; set; }
    
    [Display(Name = "Home Team")]
    public string HomeTeam { get; set; } = string.Empty;
    [Display(Name = "Away Team")]
    public string AwayTeam { get; set; } = string.Empty;
    public string Season { get; set; } = string.Empty;
    public string Referee { get; set; } = string.Empty;
    
    [Display(Name = "Home Team Goals")]
    public int HomeTeamGoals { get; set; }
    [Display(Name = "Away Team Goals")]
    public int AwayTeamGoals { get; set; }
    
    [Display(Name = "Home Team Yellow Cards")]
    public int HomeTeamYellowCards { get; set; }
    [Display(Name = "Away Team Yellow Cards")]
    public int AwayTeamYellowCards { get; set; }
    [Display(Name = "Home Team Red Cards")]
    public int HomeTeamRedCards { get; set; }
    [Display(Name = "Away Team Red Cards")]
    public int AwayTeamRedCards { get; set; }
    [Display(Name = "Match Date")]
    public DateTime OriginalDate { get; set; }
    [Display(Name = "Rescheduled")]
    public bool IsRescheduled { get; set; }
    [Display(Name = "Reschedule Reason")]
    public string? RescheduleReason { get; set; }
}
