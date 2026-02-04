using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages_Matches
{
    public class EditModel(AppDbContext context) : PageModel
    {
        private readonly AppDbContext _context = context;

        [BindProperty]
        public Match Match { get; set; } = default!;
        
        public SelectList HomeTeamList { get; set; } = null!;
        public SelectList AwayTeamList { get; set; } = null!;
        public SelectList SeasonsList { get; set; } = null!;
        public SelectList RefereesList { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(Guid? id) {
            PopulateDropDownLists();
            
            if (id == null)
            {
                return NotFound();
            }

            var match =  await _context.Matches.FirstOrDefaultAsync(m => m.Id == id);
            if (match == null)
            {
                return NotFound();
            }
            Match = match;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                PopulateDropDownLists();
                return Page();
            }
            
             
            var matches = await context.Matches.FindAsync(Match.Id);
            
            matches.HomeTeamGoals = Match.HomeTeamGoals;
            matches.AwayTeamGoals = Match.AwayTeamGoals;
            matches.HomeTeamYellowCards = Match.HomeTeamYellowCards;
            matches.AwayTeamYellowCards = Match.AwayTeamYellowCards;
            matches.HomeTeamRedCards = Match.HomeTeamRedCards;
            matches.AwayTeamRedCards = Match.AwayTeamRedCards;
            matches.OriginalDate = Match.OriginalDate;
            matches.IsRescheduled = Match.IsRescheduled;
            matches.RescheduleReason = Match.RescheduleReason;
            

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MatchExists(Match.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool MatchExists(Guid id)
        {
            return _context.Matches.Any(e => e.Id == id);
        }
        
        private async void PopulateDropDownLists()
        {
            try
            {
                HomeTeamList = new SelectList( await context.Teams.ToListAsync(), "Id", "Name");
                AwayTeamList = new SelectList( await context.Teams.ToListAsync(), "Id", "Name");
                SeasonsList = new SelectList( await context.Seasons.ToListAsync(), "Id", "Name");
                RefereesList = new SelectList( await context.Referees.ToListAsync(), "Id", "Name");
            }
            catch (Exception e) {
                throw new Exception("Can`t find Data", e);
            }
        }
    }
    
}
