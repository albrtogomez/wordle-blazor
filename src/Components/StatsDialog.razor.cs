using WordleBlazor.Model;

namespace WordleBlazor.Components
{
    public partial class StatsDialog
    {
        private Stats _stats = new();

        protected override async Task OnInitializedAsync()
        {
            var storedStats = await localStorage.GetItemAsync<Stats>(nameof(Stats) + LocalizationService.GetCurrentLanguageSuffix());

            if (storedStats != null)
                _stats = storedStats;
        }

        private string GetGamesWonPercent()
        {
            if (_stats.GamesPlayed > 0)
                return (_stats.GamesWon * 100 / _stats.GamesPlayed).ToString();
            else
                return "0";
        }

        private string GetGameResultDistributionPercent(int keyRow)
        {
            if (_stats.GamesPlayed > 0)
                return (_stats.GamesResultDistribution[keyRow] * 100 / _stats.GamesPlayed).ToString();
            else
                return "0";
        }
    }
}