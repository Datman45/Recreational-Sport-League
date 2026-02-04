using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages_TeamStandings
{
    public class EditModel(AppDbContext context) : PageModel {
        [BindProperty]
        public TeamStanding TeamStanding { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teamstanding =  await context.TeamStandings.FirstOrDefaultAsync(m => m.Id == id);
            if (teamstanding == null)
            {
                return NotFound();
            }
            TeamStanding = teamstanding;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync() {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            var teamStanding = await context.TeamStandings.FindAsync(TeamStanding.Id);
            
            teamStanding.Wins = TeamStanding.Wins;
            teamStanding.Losses = TeamStanding.Losses;
            teamStanding.Draws = TeamStanding.Draws;
            teamStanding.GoalsFor = TeamStanding.GoalsFor;
            teamStanding.GoalsAgainst = TeamStanding.GoalsAgainst;
            teamStanding.FairPlayPoints = TeamStanding.FairPlayPoints;
            
            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeamStandingExists(TeamStanding.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToPage("./Index");
        }

        private bool TeamStandingExists(Guid id)
        {
            return context.TeamStandings.Any(e => e.Id == id);
        }
    }
}
