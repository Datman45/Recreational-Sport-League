using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Domain;

namespace WebApp.Pages_Referees
{
    public class IndexModel(DAL.AppDbContext context) : PageModel
    {
        public IList<Referee> Referee { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Referee = await context.Referees.ToListAsync();
        }
    }
}
