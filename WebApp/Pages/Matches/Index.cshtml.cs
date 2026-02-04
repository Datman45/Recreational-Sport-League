using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Pages_Matches
{
    public class IndexModel(DAL.AppDbContext context) : PageModel
    {
        public IList<Match> Match { get;set; } = default!;
     

        public async Task OnGetAsync()
        {
            Match = await context.Matches.ToListAsync();
        }
        
        public async Task<IActionResult> OnPostToggleRescheduledAsync(Guid id, bool isRescheduled) {
            var match =  await context.Matches.FindAsync(id);
            if (match == null) return id == Guid.Empty ? RedirectToPage()
                : RedirectToPage(new {id});
            match.IsRescheduled = isRescheduled;
            match.RescheduleReason = string.Empty;
            await context.SaveChangesAsync();
            return id == Guid.Empty ? RedirectToPage() : RedirectToPage(new {id});
        }
    }
}
