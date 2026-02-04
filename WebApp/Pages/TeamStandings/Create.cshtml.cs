using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages_TeamStandings
{
    public class CreateModel(AppDbContext context) : PageModel
    {
        public SelectList? Teams { get; set; }
        public SelectList? Seasons { get; set; }
        public SelectList? TeamStandings { get; set; }
        public string ErrorMessage { get; set; } = "";
        public async Task<IActionResult> OnGetAsync() {
            await PopulateDropDownLists();
            return Page();
        }

        [BindProperty]
        public TeamStanding TeamStanding { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync() {
            await PopulateDropDownLists();
            
            if (TeamStandings != null && TeamStandings.Any(selectListItem => selectListItem.Value == TeamStanding.TeamId.ToString())) {
                ErrorMessage = "Team already exists in the standings.";
                return Page();
            }
            
            var team = await context.Teams.FindAsync(TeamStanding.TeamId);
            if (team != null) TeamStanding.TeamName = team.Name;
            
            var season = await context.Seasons.FindAsync(TeamStanding.SeasonId);
            if (season != null) TeamStanding.SeasonName = season.Name;

            TeamStanding.Wins = 0;
            TeamStanding.Losses = 0;
            TeamStanding.Draws = 0;
            TeamStanding.GoalsFor = 0;
            TeamStanding.GoalsAgainst = 0;
            TeamStanding.FairPlayPoints = 100;
            
            if (!ModelState.IsValid)
            {
                await PopulateDropDownLists();
                return Page();
            }

            context.TeamStandings.Add(TeamStanding);
            await context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        private async Task PopulateDropDownLists() {
            TeamStandings = new SelectList(await context.TeamStandings.ToListAsync(), "TeamId", "TeamName");
            Teams = new SelectList( await context.Teams.ToListAsync(), "Id", "Name");
            Seasons = new SelectList( await context.Seasons.ToListAsync(), "Id", "Name");
        }
    }
}
