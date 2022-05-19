using BlazorComponentUtilities;
using WordleBlazor.Models.Enums;

namespace WordleBlazor.Components
{
    public partial class GameBoard
    {
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

        protected override async Task OnInitializedAsync()
        {
            await GameManagerService.LoadJsonSpanishDictionary();
            await GameManagerService.GetTodaySolution();
            GameManagerService.StartGame();
        }

        public void NotifyChange()
        {
            InvokeAsync(StateHasChanged);
        }

        private string GetBoardCellStateCssClass(BoardCellState state)
        {
            return state switch
            {
                BoardCellState.Typing => "text-black border-darkgray",
                BoardCellState.Correct => "bg-green border-green",
                BoardCellState.IncorrectPosition => "bg-yellow border-yellow",
                BoardCellState.Wrong => "bg-darkgray border-darkgray",
                _ => ""
            };
        }

        private string GetNextWordMessage()
        {
            if (GameManagerService.GameState == GameState.Win)
                return "Enhorabuena, has ganado!";
            else
                return "Una lástima, has perdido!";
        }

        private void PlayAgain()
        {
            GameManagerService.Reset();
        }
    }
}