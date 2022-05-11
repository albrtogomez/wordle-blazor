using WordleBlazor.Models.Enums;

namespace WordleBlazor.Models
{
    public class BoardCell
    {
        public char? Value { get; set; }
        public BoardCellState State { get; set; } = BoardCellState.Empty;
    }
}
