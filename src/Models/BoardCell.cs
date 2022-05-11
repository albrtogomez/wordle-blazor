using WordleBlazor.Models.Enums;

namespace WordleBlazor.Models
{
    public class BoardCell
    {
        public char? Value { get; set; }
        public BoardCellStatus Status { get; set; } = BoardCellStatus.Empty;
    }
}
