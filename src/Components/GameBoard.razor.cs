using WordleBlazor.Models.Enums;

namespace WordleBlazor.Components
{
    public partial class GameBoard
    {
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
    }
}