using Blazored.LocalStorage;
using WordleBlazor.Model;
using WordleBlazor.Model.Enums;

namespace WordleBlazor.Services
{
    public class BrowserLocalStorageService
    {
        public DateTime GameStarted { get; set; }

        public DateTime LastGamePlayedDate { get; set; }

        private readonly ILocalStorageService _localStorage;
        private readonly LocalizationService _localizationService;

        public BrowserLocalStorageService(ILocalStorageService localStorage, LocalizationService localizationService)
        {
            _localStorage = localStorage;
            _localizationService = localizationService;
        }

        public async Task<List<string>?> LoadGameStateFromLocalStorage()
        {
            DateTime localStorageLastDayPlayed = await _localStorage.GetItemAsync<DateTime>(nameof(LastGamePlayedDate) + _localizationService.GetCurrentLanguageSuffix());
            var today = DateTime.Now.Date;

            if (localStorageLastDayPlayed == today)
            {
                LastGamePlayedDate = localStorageLastDayPlayed;

                var board = await _localStorage.GetItemAsync<List<string>>("BoardGrid" + _localizationService.GetCurrentLanguageSuffix());
                
                if (board != null)
                {
                    return board;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                LastGamePlayedDate = GameStarted.Date;
                await _localStorage.SetItemAsync(nameof(LastGamePlayedDate) + _localizationService.GetCurrentLanguageSuffix(), GameStarted.Date);
                await _localStorage.RemoveItemAsync("BoardGrid" + _localizationService.GetCurrentLanguageSuffix());

                return null;
            }
        }

        public async Task SaveCurrentBoardToLocalStorage(List<string> boardGridWords)
        {
            await _localStorage.SetItemAsync("BoardGrid" + _localizationService.GetCurrentLanguageSuffix(), boardGridWords);
        }

        public async Task UpdateGameStats(GameState gameState, int currentRow)
        {
            var lastGameFinishedDate = await _localStorage.GetItemAsync<DateTime>("lastGameFinishedDate" + _localizationService.GetCurrentLanguageSuffix());
            var today = DateTime.Now.Date;

            if (lastGameFinishedDate != today)
            {
                var stats = await _localStorage.GetItemAsync<Stats>(nameof(Stats) + _localizationService.GetCurrentLanguageSuffix());
                if (stats == null)
                {
                    stats = new Stats();
                }

                stats.GamesPlayed++;

                if (gameState == GameState.Win)
                {
                    stats.GamesWon++;
                    stats.CurrentStreak++;

                    if (stats.CurrentStreak > stats.BestStreak)
                        stats.BestStreak = stats.CurrentStreak;

                    stats.GamesResultDistribution[currentRow + 1]++;
                }
                else
                {
                    stats.CurrentStreak = 0;
                    stats.GamesResultDistribution[-1]++;
                }

                await _localStorage.SetItemAsync(nameof(Stats) + _localizationService.GetCurrentLanguageSuffix(), stats);
            }
        }

        public async Task SaveLastGameFinishedDate()
        {
            await _localStorage.SetItemAsync("lastGameFinishedDate" + _localizationService.GetCurrentLanguageSuffix(), LastGamePlayedDate);
        }
    }
}
