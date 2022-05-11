using System.Net.Http.Json;
using System.Text;
using WordleBlazor.Models;
using WordleBlazor.Models.Enums;

namespace WordleBlazor.Services
{
    public class GameManagerService
    {
        public static readonly int RowSize = 6;
        public static readonly int ColumnSize = 5;

        private BoardCell[,] _boardGrid;
        public BoardCell[,] BoardGrid { get => _boardGrid; }

        private readonly HttpClient _httpClient;

        private string solution = "CALDO"; // IMPORTANT: COLUMN SIZE MUST BE EQUALS TO SOLUTION LENGHT!
        private List<string> validWords = new();
        private int currentRow;
        private int currentColumn;
        private GameState gameStatus;

        public GameManagerService(HttpClient httpClient)
        {
            _httpClient = httpClient;

            _boardGrid = new BoardCell[RowSize, ColumnSize];

            PopulateBoard();
        }

        public async Task LoadJsonSpanishDictionary()
        {
            var wordList = await _httpClient.GetFromJsonAsync<List<string>>("data/spanish-words.json");

            validWords = wordList ?? new List<string>();
        }

        public void StartGame()
        {
            gameStatus = GameState.Playing;
        }

        public void Reset()
        {
            _boardGrid = new BoardCell[RowSize, ColumnSize];

            PopulateBoard();

            currentRow = 0;
            currentColumn = 0;

            gameStatus = GameState.Playing;
        }

        public void EnterNextValue(char value)
        {
            if (gameStatus == GameState.Playing)
            {
                if (currentColumn == ColumnSize - 1 && BoardGrid[currentRow, currentColumn].Value != null)
                    return;

                BoardGrid[currentRow, currentColumn].Value = value;
                BoardGrid[currentRow, currentColumn].State = BoardCellState.Typing;

                if (currentColumn < ColumnSize - 1)
                    currentColumn++;
            }
        }

        public void RemoveLastValue()
        {
            if (gameStatus == GameState.Playing)
            {
                if (currentColumn == 0 && BoardGrid[currentRow, currentColumn].Value == null)
                    return;

                if (currentColumn <= ColumnSize - 1 && BoardGrid[currentRow, currentColumn].Value == null)
                {
                    BoardGrid[currentRow, currentColumn - 1].Value = null;
                    BoardGrid[currentRow, currentColumn - 1].State = BoardCellState.Empty;
                    currentColumn--;
                }
                else
                {
                    BoardGrid[currentRow, currentColumn].Value = null;
                    BoardGrid[currentRow, currentColumn].State = BoardCellState.Empty;
                }
            }
        }

        public void CheckCurrentLineSolution()
        {
            if (gameStatus == GameState.Playing)
            {
                StringBuilder currentLineBuilder = new();

                for (int i = 0; i < ColumnSize; i++)
                {
                    currentLineBuilder.Append(BoardGrid[currentRow, i].Value);
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
                        _boardGrid[currentRow, i].State = BoardCellState.Wrong;
                    else if (index == i)
                        _boardGrid[currentRow, i].State = BoardCellState.Correct;
                    else
                        _boardGrid[currentRow, i].State = BoardCellState.IncorrectPosition;
                }

                if (currentLine == solution)
                {
                    gameStatus = GameState.Win;
                    // Win message
                }
                else if (currentRow < RowSize - 1)
                {
                    currentRow++;
                    currentColumn = 0;
                }
                else
                {
                    gameStatus = GameState.GameOver;
                    // Game Over Message (with solution)
                }
            }
        }

        private void PopulateBoard()
        {
            for (int i = 0; i < RowSize; i++)
            {
                for (int j = 0; j < ColumnSize; j++)
                {
                    _boardGrid[i, j] = new BoardCell();
                }
            }
        }
    }
}
