using Microsoft.AspNetCore.Components;
using WordleBlazor.Models.Enums;

namespace WordleBlazor.Components
{
    public partial class BoardCell
    {
        private string TriggerFlipClass = "";
        private string FlipDelay => ColumnIndex > 0 ? $"{ColumnIndex * 200}ms" : "";

        [Parameter, EditorRequired]
        public char? CellValue { get; set; }

        private BoardCellState _cellState;
        [Parameter, EditorRequired]
        public BoardCellState CellState
        {
            get
            {
                return _cellState;
            }
            set
            {
                if (value == BoardCellState.Empty)
                    TriggerFlipClass = "";

                _cellState = value;
            }
        }

        [Parameter, EditorRequired]
        public int RowIndex { get; set; }

        [Parameter, EditorRequired]
        public int ColumnIndex { get; set; }

        protected override void OnInitialized()
        {
            GameManagerService.OnCurrentLineCheckedSolution += TriggerFlip;
        }

        public void Dispose()
        {
            GameManagerService.OnCurrentLineCheckedSolution -= TriggerFlip;
        }

        private void TriggerFlip(int currentRow)
        {
            if (currentRow == RowIndex)
            {
                if (string.IsNullOrEmpty(TriggerFlipClass))
                    TriggerFlipClass = "doflip";
                else
                    TriggerFlipClass = "";
            }
        }

        private string GetFrontBoardCellStateCssClass()
        {
            return CellState != BoardCellState.Empty ? "text-black border-darkgray" : "";
        }

        private string GetBackBoardCellStateCssClass()
        {
            return CellState switch
            {
                BoardCellState.Typing => "text-black border-darkgray",
                BoardCellState.Correct => "bg-green border-green",
                BoardCellState.IncorrectPosition => "bg-yellow border-yellow",
                BoardCellState.Wrong => "bg-darkgray border-darkgray",
                _ => ""
            };
        }
    }
}