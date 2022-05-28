using WordleBlazor.Models;

namespace WordleBlazor.Components
{
    public partial class StatsDialog
    {
        private Stats stats = null!;

        protected override async Task OnInitializedAsync()
        {
            stats = await localStorage.GetItemAsync<Stats>(nameof(Stats));

            if (stats == null)
                stats = new Stats();
        }

        private string GetGamesWonPercent()
        {
            return (stats.GamesWon * 100 / @stats.GamesPlayed).ToString();
        }

        private string GetGameResultDistributionPercent(int keyRow)
        {
            return (stats.GamesResultDistribution[keyRow] * 100 / @stats.GamesPlayed).ToString();
        }
    }
}