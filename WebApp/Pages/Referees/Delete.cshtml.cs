using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Domain;

namespace WebApp.Pages_Referees
{
    public class DeleteModel(DAL.AppDbContext context) : PageModel
    {
        [BindProperty]
        public Referee Referee { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var referee = await context.Referees.FirstOrDefaultAsync(m => m.Id == id);

            if (referee is not null)
            {
                Referee = referee;

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

            var referee = await context.Referees.FindAsync(id);
            if (referee != null)
            {
                Referee = referee;
                context.Referees.Remove(Referee);
                await context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
