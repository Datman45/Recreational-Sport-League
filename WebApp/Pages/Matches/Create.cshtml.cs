using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages_Matches
{
    public class CreateModel(AppDbContext context) : PageModel
    {
        public SelectList HomeTeamList { get; set; } = null!;
        public SelectList AwayTeamList { get; set; } = null!;
        public SelectList SeasonsList { get; set; } = null!;
        public SelectList RefereesList { get; set; } = null!;

        [BindProperty]
        public Match Match { get; set; } = null!;

        public string ErrorMessage { get; set; } = "";

        public async Task<IActionResult> OnGetAsync()
        {
            await PopulateDropDownLists();
            return Page();
        }

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            var matches = await context.Matches.ToListAsync();
            var teams = await context.Teams.ToListAsync();
            var seasons = await context.Seasons.ToListAsync();

            // Same team
            if (Match.HomeTeamId == Match.AwayTeamId)
            {
                ErrorMessage = "Home team and away team cant be the same";
                await PopulateDropDownLists();
                return Page();
            }

            // Match date on the weekend
            if (Match.OriginalDate.DayOfWeek != DayOfWeek.Saturday &&
                Match.OriginalDate.DayOfWeek != DayOfWeek.Sunday)
            {
                ErrorMessage = "Match date must be Saturday or Sunday";
                await PopulateDropDownLists();
                return Page();
            }

            // Matches not in the season
            if (seasons.Any(i =>
                    i.Id == Match.SeasonId &&
                    (Match.OriginalDate < i.StartDate || Match.OriginalDate > i.EndDate)))
            {
                ErrorMessage = "You cant put matches not in this season";
                await PopulateDropDownLists();
                return Page();
            }

            // 2 matches on the same weekends (back-to-back days)
            if (matches.Any(i =>
                    (i.HomeTeamId == Match.HomeTeamId ||
                     i.AwayTeamId == Match.AwayTeamId ||
                     i.HomeTeamId == Match.AwayTeamId ||
                     i.AwayTeamId == Match.AwayTeamId) &&
                    Math.Abs((i.OriginalDate.Date - Match.OriginalDate.Date).Days) == 1))
            {
                ErrorMessage = "Team cannot play back-to-back days";
                await PopulateDropDownLists();
                return Page();
            }

            // More than 2 games in a row (away matches)
            var teamMatches = matches
                .Where(i => i.AwayTeamId == Match.AwayTeamId || i.HomeTeamId == Match.AwayTeamId)
                .OrderByDescending(i => i.OriginalDate)
                .ToList();

            var consecutiveAway = 0;
            foreach (var teamMatch in teamMatches)
            {
                if (teamMatch.AwayTeamId == Match.AwayTeamId)
                {
                    consecutiveAway++;
                }
                else
                {
                    break;
                }
            }

            if (consecutiveAway >= 2)
            {
                ErrorMessage = "More than 2 away matches";
                await PopulateDropDownLists();
                return Page();
            }

            // Referee double adding on the same day
            if (matches.Any(i =>
                    i.RefereeId == Match.RefereeId &&
                    i.OriginalDate.Date == Match.OriginalDate.Date))
            {
                ErrorMessage = "Referee already assigned to the match on this day";
                await PopulateDropDownLists();
                return Page();
            }

            // Impossible to play in the evening because there are no lights
            if (teams.Any(i =>
                    i.Id == Match.HomeTeamId &&
                    !i.HasLights &&
                    Match.OriginalDate.Hour >= 18))
            {
                ErrorMessage = "Home team doesn't have lights and cant play in evening";
                await PopulateDropDownLists();
                return Page();
            }

            var homeTeam = await context.Teams.FindAsync(Match.HomeTeamId);
            var awayTeam = await context.Teams.FindAsync(Match.AwayTeamId);
            var season = await context.Seasons.FindAsync(Match.SeasonId);
            var referee = await context.Referees.FindAsync(Match.RefereeId);

            if (homeTeam != null) Match.HomeTeam = homeTeam.Name;
            if (awayTeam != null) Match.AwayTeam = awayTeam.Name;
            if (season != null) Match.Season = season.Name;
            if (referee != null) Match.Referee = referee.Name;

            Match.IsRescheduled = false;
            Match.RescheduleReason = "";
            Match.HomeTeamGoals = 0;
            Match.AwayTeamGoals = 0;
            Match.HomeTeamYellowCards = 0;
            Match.AwayTeamYellowCards = 0;
            Match.HomeTeamRedCards = 0;
            Match.AwayTeamRedCards = 0;

            if (!ModelState.IsValid)
            {
                await PopulateDropDownLists();
                return Page();
            }

            context.Matches.Add(Match);
            await context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        private async Task PopulateDropDownLists()
        {
            try
            {
                HomeTeamList = new SelectList(await context.Teams.ToListAsync(), "Id", "Name");
                AwayTeamList = new SelectList(await context.Teams.ToListAsync(), "Id", "Name");
                SeasonsList = new SelectList(await context.Seasons.ToListAsync(), "Id", "Name");
                RefereesList = new SelectList(await context.Referees.ToListAsync(), "Id", "Name");
            }
            catch (Exception e)
            {
                throw new Exception("Cant find Data", e);
            }
        }
    }
}
