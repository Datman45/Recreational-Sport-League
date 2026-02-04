using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages_TeamStandings
{
    public class DeleteModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public DeleteModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public TeamStanding TeamStanding { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teamstanding = await _context.TeamStandings.FirstOrDefaultAsync(m => m.Id == id);

            if (teamstanding is not null)
            {
                TeamStanding = teamstanding;

                return Page();
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teamstanding = await _context.TeamStandings.FindAsync(id);
            if (teamstanding != null)
            {
                TeamStanding = teamstanding;
                _context.TeamStandings.Remove(TeamStanding);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
