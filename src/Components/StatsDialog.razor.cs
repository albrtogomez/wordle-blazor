using WordleBlazor.Models;

namespace WordleBlazor.Components
{
    public partial class StatsDialog
    {
        private Stats stats = new();

        protected override async Task OnInitializedAsync()
        {
            var currentCulture = await localStorage.GetItemAsync<string>("CurrentCulture");

            string cultureSuffix;

            if (currentCulture?.StartsWith("es") == true)
                cultureSuffix = "-ES";
            else
                cultureSuffix = "-EN";

            var storedStats = await localStorage.GetItemAsync<Stats>(nameof(Stats) + cultureSuffix);

            if (storedStats != null)
                stats = storedStats;
        }

        private string GetGamesWonPercent()
        {
            if (stats.GamesPlayed > 0)
                return (stats.GamesWon * 100 / stats.GamesPlayed).ToString();
            else
                return "0";
        }

        private string GetGameResultDistributionPercent(int keyRow)
        {
            if (stats.GamesPlayed > 0)
                return (stats.GamesResultDistribution[keyRow] * 100 / stats.GamesPlayed).ToString();
            else
                return "0";
        }
    }
}