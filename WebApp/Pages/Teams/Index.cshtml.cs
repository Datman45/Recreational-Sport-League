using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Pages_Teams
{
    public class IndexModel(DAL.AppDbContext context) : PageModel
    {
        public IList<Team> Teams { get;set; } = null!;
        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public int Count { get; set; }
        public int PageSize { get; set; } = 5;
        
        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));

        public async Task OnGetAsync()
        {
            var query = context.Teams.AsQueryable();
            
            if (!string.IsNullOrEmpty(SearchString)) {
                query = query.Where(ts => ts.Name.ToLower().Contains(SearchString.ToLower()));
            }
            
            Count = await query.CountAsync();
            
            Teams = await query
                .OrderBy(i => i.Name)
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostToggleLightsAsync(Guid id, bool lights) {
            var team =  await context.Teams.FindAsync(id);
            team?.HasLights = lights;
            await context.SaveChangesAsync();
            return RedirectToPage();
        }
    }
}
