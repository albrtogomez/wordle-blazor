using BlazorComponentUtilities;
using Microsoft.AspNetCore.Components;
using System.Timers;
using WordleBlazor.Model.Enums;
using WordleBlazor.Pages;

namespace WordleBlazor.Components
{
    public partial class GameBoard
    {
        [Parameter, EditorRequired]
        public Wordle? AncestorComponent { get; set; }

        private string? NextWordClasses => new CssBuilder()
            .AddClass("flex", ShowNextWord)
            .AddClass("hidden", !ShowNextWord)
            .Build();

        private string? KeyboardContainerClasses => new CssBuilder()
            .AddClass("flex", ShowKeyboard)
            .AddClass("hidden", !ShowKeyboard)
            .Build();

        private bool ShowNextWord => GameManagerService.GameState == GameState.Win ||
                GameManagerService.GameState == GameState.GameOver;

        private bool ShowKeyboard => GameManagerService.GameState == GameState.NotStarted ||
                GameManagerService.GameState == GameState.Playing;

        private string timeLeftForNextWord = "00:00:00";
        private System.Timers.Timer currentTimeUpdaterTimer = null!;

        protected override async Task OnInitializedAsync()
        {
            await GameManagerService.LoadGameData();

            currentTimeUpdaterTimer = new System.Timers.Timer(1000);
            currentTimeUpdaterTimer.Elapsed += UpdateCurrentTime;
            currentTimeUpdaterTimer.Start();

            await GameManagerService.StartGame();
        }

        public void NotifyChange()
        {
            InvokeAsync(StateHasChanged);
        }

        private void UpdateCurrentTime(object? sender, ElapsedEventArgs e)
        {
            timeLeftForNextWord = (DateTime.Today.AddDays(1) - DateTime.Now).ToString(@"hh\:mm\:ss");

            InvokeAsync(StateHasChanged);
        }

        private string GetNextWordMessage()
        {
            if (GameManagerService.GameState == GameState.Win)
                return LocalizationService["GameboardWin"];
            else
                return LocalizationService["GameboardLose"];
        }

        private void ShowStats()
        {
            AncestorComponent?.ShowStats();
        }
    }
}