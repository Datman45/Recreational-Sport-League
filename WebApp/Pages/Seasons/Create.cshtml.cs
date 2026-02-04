using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages_Seasons
{
    public class CreateModel(DAL.AppDbContext context) : PageModel
    {
        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Season Season { get; set; } = default!;

        public string ErrorMessage = "";

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync() {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var seasons = new SelectList(await context.Seasons.ToListAsync(), "Id", "Name");
            
            if (seasons.Any(selectListItem => selectListItem.Text == Season.Name)) {
                ErrorMessage = "Season Name is already taken";
                return Page();
            }

            if (Season.EndDate < Season.StartDate) {
                ErrorMessage = "End date must be after start date";
                return Page();
            }
            

            context.Seasons.Add(Season);
            await context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
