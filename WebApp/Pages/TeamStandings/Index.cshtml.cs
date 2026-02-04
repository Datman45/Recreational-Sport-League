using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages_TeamStandings;

public class IndexModel(AppDbContext context) : PageModel
{
    public IList<TeamStanding> TeamStandings { get;set; } = null!;
    [BindProperty(SupportsGet = true)]
    public string? SearchString { get; set; }
    [BindProperty(SupportsGet = true)]
    public int CurrentPage { get; set; } = 1;
    public int Count { get; set; }
    public int PageSize { get; set; } = 5;
        
    public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));

    public async Task OnGetAsync()
    {
        var query = context.TeamStandings.AsQueryable();
            
        if (!string.IsNullOrEmpty(SearchString)) {
            query = query.Where(ts => ts.TeamName.ToLower().Contains(SearchString.ToLower()));
        }
            
        Count = await query.CountAsync();
            
        TeamStandings = await query
            .OrderBy(i => i.TeamName)
            .Skip((CurrentPage - 1) * PageSize)
            .Take(PageSize)
            .ToListAsync();

        foreach (var teamStanding in TeamStandings) {
            var homeMatches = await context.Matches.Where(i => i.HomeTeamId == teamStanding.TeamId).ToListAsync();
            foreach (var match in homeMatches) {
                teamStanding.GoalsFor += match.HomeTeamGoals;
                teamStanding.GoalsAgainst += match.AwayTeamGoals;
                teamStanding.Wins += match.HomeTeamGoals > match.AwayTeamGoals ? 1 : 0;
                teamStanding.Losses += match.HomeTeamGoals < match.AwayTeamGoals ? 1 : 0;
                teamStanding.Draws += match.HomeTeamGoals == match.AwayTeamGoals ? 1 : 0;
                teamStanding.FairPlayPoints -= match.HomeTeamYellowCards + match.HomeTeamRedCards * 3;
            }
                
            var awayMatches = await context.Matches.Where(i => i.AwayTeamId == teamStanding.TeamId).ToListAsync();
            foreach (var match in awayMatches) {
                teamStanding.GoalsFor += match.AwayTeamGoals;
                teamStanding.GoalsAgainst += match.HomeTeamGoals;
                teamStanding.Wins += match.AwayTeamGoals > match.HomeTeamGoals ? 1 : 0;
                teamStanding.Losses += match.AwayTeamGoals < match.HomeTeamGoals ? 1 : 0;
                teamStanding.Draws += match.AwayTeamGoals == match.HomeTeamGoals ? 1 : 0;
                teamStanding.FairPlayPoints -= match.AwayTeamYellowCards + match.AwayTeamRedCards * 3;
            }
        }
    }
}