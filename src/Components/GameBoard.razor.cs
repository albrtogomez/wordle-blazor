using BlazorComponentUtilities;
using WordleBlazor.Models.Enums;

namespace WordleBlazor.Components
{
    public partial class GameBoard
    {
        private string? NextWordClasses => new CssBuilder()
            .AddClass("next-word")
            .AddClass("show", GameManagerService.GameState == GameState.Win ||
                GameManagerService.GameState == GameState.GameOver)
            .Build();

        private string? KeyboardContainerClasses => new CssBuilder()
            .AddClass("keyboard-container")
            .AddClass("show", GameManagerService.GameState == GameState.NotStarted ||
                GameManagerService.GameState == GameState.Playing)
            .Build();

        protected override async Task OnInitializedAsync()
        {
            await GameManagerService.LoadJsonSpanishDictionary();
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
                BoardCellState.Typing => "board-typing-letter",
                BoardCellState.Correct => "board-correct-letter",
                BoardCellState.IncorrectPosition => "board-incorrectposition-letter",
                BoardCellState.Wrong => "board-wrong-letter",
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