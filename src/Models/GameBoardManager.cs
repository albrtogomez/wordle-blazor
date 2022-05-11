using System.Text;
using WordleBlazor.Models.Enums;

namespace WordleBlazor.Models
{
    public class GameBoardManager
    {
        public static readonly int RowSize = 6;
        public static readonly int ColumnSize = 5;

        private BoardCell[,] _board;
        public BoardCell[,] Board { get => _board; }

        private string solution = "CALDO"; // IMPORTANT: COLUMN SIZE MUST BE EQUALS TO SOLUTION LENGHT!
        private List<string> validWords = new() { "AIREO", "SANTO", "CAMPO" };
        
        private int currentRow;
        private int currentColumn;

        private GameStatus gameStatus;

        public GameBoardManager()
        {
            _board = new BoardCell[RowSize, ColumnSize];

            PopulateBoard();

            gameStatus = GameStatus.Playing;
        }

        private void PopulateBoard()
        {
            for (int i = 0; i < RowSize; i++)
            {
                for (int j = 0; j < ColumnSize; j++)
                {
                    _board[i, j] = new BoardCell();
                }
            }
        }

        public void Reset()
        {
            _board = new BoardCell[RowSize, ColumnSize];

            PopulateBoard();

            currentRow = 0;
            currentColumn = 0;

            gameStatus = GameStatus.Playing;
        }

        public void EnterNextValue(char value)
        {
            if (gameStatus == GameStatus.Playing)
            {
                if (currentColumn == ColumnSize - 1 && Board[currentRow, currentColumn].Value != null)
                    return;

                Board[currentRow, currentColumn].Value = value;
                Board[currentRow, currentColumn].Status = BoardCellStatus.Typing;

                if (currentColumn < ColumnSize - 1)
                    currentColumn++;
            }
        }

        public void RemoveLastValue()
        {
            if (gameStatus == GameStatus.Playing)
            {
                if (currentColumn == 0 && Board[currentRow, currentColumn].Value == null)
                    return;

                if (currentColumn <= ColumnSize - 1 && Board[currentRow, currentColumn].Value == null)
                {
                    Board[currentRow, currentColumn - 1].Value = null;
                    Board[currentRow, currentColumn - 1].Status = BoardCellStatus.Empty;
                    currentColumn--;
                }
                else
                {
                    Board[currentRow, currentColumn].Value = null;
                    Board[currentRow, currentColumn].Status = BoardCellStatus.Empty;
                }
            }
        }

        public void CheckCurrentLineSolution()
        {
            if (gameStatus == GameStatus.Playing)
            {
                StringBuilder currentLineBuilder = new();

                for (int i = 0; i < ColumnSize; i++)
                {
                    currentLineBuilder.Append(Board[currentRow, i].Value);
                }

                string currentLine = currentLineBuilder.ToString();

                if (currentLine.Length != solution.Length)
                    return; // Not enough letters message

                if (!validWords.Contains(currentLine) && currentLine != solution)
                    return; // Not valid word message

                for (int i = 0; i < currentLine.Length; i++)
                {
                    var index = solution.IndexOf(currentLine[i]);

                    if (index < 0)
                        _board[currentRow, i].Status = BoardCellStatus.Wrong;
                    else if (index == i)
                        _board[currentRow, i].Status = BoardCellStatus.Correct;
                    else
                        _board[currentRow, i].Status = BoardCellStatus.IncorrectPosition;
                }

                if (currentLine == solution)
                {
                    gameStatus = GameStatus.Win;
                    // Win message
                }
                else if (currentRow < RowSize - 1)
                {
                    currentRow++;
                    currentColumn = 0;
                }
                else
                {
                    gameStatus = GameStatus.GameOver;
                    // Game Over Message (with solution)
                }
            }
        }
    }
}
