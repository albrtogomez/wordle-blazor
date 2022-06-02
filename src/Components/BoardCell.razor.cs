using Microsoft.AspNetCore.Components;
using WordleBlazor.Model.Enums;

namespace WordleBlazor.Components
{
    public partial class BoardCell
    {
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
                    _triggerFlipClass = "";

                _cellState = value;
            }
        }

        [Parameter, EditorRequired]
        public int RowIndex { get; set; }

        [Parameter, EditorRequired]
        public int ColumnIndex { get; set; }

        private string FlipDelay => ColumnIndex > 0 ? $"{ColumnIndex * 200}ms" : "";

        private string _triggerFlipClass = "";

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
                if (string.IsNullOrEmpty(_triggerFlipClass))
                    _triggerFlipClass = "doflip";
                else
                    _triggerFlipClass = "";
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