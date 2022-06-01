using WordleBlazor.Model.Enums;

namespace WordleBlazor.Model
{
    public class BoardCell
    {
        public char? Value { get; set; }
        public BoardCellState State { get; set; } = BoardCellState.Empty;
    }
}
