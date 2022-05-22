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

        public event Action<int> OnBoardLineWrongSolution = default!;
        public event Action<int> OnCurrentLineCheckedSolution = default!;

        private BoardCell[,] _boardGrid;
        public BoardCell[,] BoardGrid { get => _boardGrid; }

        public Dictionary<char, KeyState> UsedKeys { get; set; } = new Dictionary<char, KeyState>();

        public GameState _gameState;
        public GameState GameState { get => _gameState; }

        private readonly HttpClient _httpClient;
        private readonly ToastNotificationService _toastNotificationService;

        private string solution = "";
        private List<string> validWords = new();
        private int currentRow;
        private int currentColumn;

        public GameManagerService(HttpClient httpClient, ToastNotificationService toastNotificationService)
        {
            _httpClient = httpClient;
            _toastNotificationService = toastNotificationService;

            _boardGrid = new BoardCell[RowSize, ColumnSize];

            PopulateBoard();
        }

        public async Task LoadJsonSpanishDictionary()
        {
            var wordList = await _httpClient.GetFromJsonAsync<List<string>>("data/spanish-words.json");

            validWords = wordList ?? new List<string>();
        }

        public async Task GetTodaySolution()
        {
            var solutions = await _httpClient.GetFromJsonAsync<Dictionary<int, string>>("data/daily-solutions-sp.json");

            if (solutions != null)
            {
                solution = solutions[DateTime.Now.DayOfYear];
            }
        }

        public void StartGame()
        {
            _gameState = GameState.Playing;
        }

        public void Reset()
        {
            _boardGrid = new BoardCell[RowSize, ColumnSize];
            UsedKeys = new Dictionary<char, KeyState>();

            PopulateBoard();

            currentRow = 0;
            currentColumn = 0;

            _gameState = GameState.Playing;
        }

        public void EnterNextValue(char value)
        {
            if (GameState == GameState.Playing)
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
            if (GameState == GameState.Playing)
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
            if (GameState == GameState.Playing)
            {
                StringBuilder currentLineBuilder = new();

                for (int i = 0; i < ColumnSize; i++)
                {
                    currentLineBuilder.Append(BoardGrid[currentRow, i].Value);
                }

                string currentLine = currentLineBuilder.ToString();

                if (currentLine.Length != solution.Length)
                {
                    _toastNotificationService.ShowToast("No hay suficientes letras");
                    OnBoardLineWrongSolution.Invoke(currentRow);
                    return;
                }

                if (!validWords.Contains(currentLine) && currentLine != solution)
                {
                    _toastNotificationService.ShowToast("La palabra no existe");
                    OnBoardLineWrongSolution.Invoke(currentRow);
                    return;
                }

                for (int i = 0; i < currentLine.Length; i++)
                {
                    var foundIndexes = new List<int>();

                    for (int j = 0; j < solution.Length; j++)
                    {
                        if (solution[j] == currentLine[i])
                            foundIndexes.Add(j);
                    }

                    if (foundIndexes.Count > 0)
                    {
                        if (foundIndexes.Contains(i))
                        {
                            _boardGrid[currentRow, i].State = BoardCellState.Correct;

                            if (!UsedKeys.TryAdd(currentLine[i], KeyState.Correct))
                                UsedKeys[currentLine[i]] = KeyState.Correct;
                        }
                        else
                        {
                            if (foundIndexes.Count > GetCurrentRowCorrectCellsFromValue(foundIndexes, currentLine[i]))
                            {
                                if (foundIndexes.Count > GetCurrentRowIncorrectPositionCellsFromValue(currentLine[i]))
                                {
                                    _boardGrid[currentRow, i].State = BoardCellState.IncorrectPosition;
                                }
                                else
                                {
                                    _boardGrid[currentRow, i].State = BoardCellState.Wrong;
                                }
                            }
                            else
                            {
                                _boardGrid[currentRow, i].State = BoardCellState.Wrong;
                            }

                            UsedKeys.TryAdd(currentLine[i], KeyState.IncorrectPosition);
                        }
                    }
                    else
                    {
                        _boardGrid[currentRow, i].State = BoardCellState.Wrong;
                        UsedKeys.TryAdd(currentLine[i], KeyState.Wrong);
                    }
                }

                OnCurrentLineCheckedSolution.Invoke(currentRow);

                if (currentLine == solution)
                {
                    _gameState = GameState.Win;
                }
                else if (currentRow < RowSize - 1)
                {
                    currentRow++;
                    currentColumn = 0;
                }
                else
                {
                    _gameState = GameState.GameOver;
                }
            }
        }

        public int GetCurrentRow()
        {
            return currentRow;
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

        private int GetCurrentRowCorrectCellsFromValue(List<int> foundIndexes, char value)
        {
            var valueCells = Enumerable.Range(0, _boardGrid.GetLength(1))
                .Select(x => _boardGrid[currentRow, x])
                .ToArray();

            int count = 0;
            
            for (int i = 0; i < valueCells.Length; i++)
            {
                if (valueCells[i].Value == value && foundIndexes.Contains(i))
                    count++;
            }

            return count;
        }

        private int GetCurrentRowIncorrectPositionCellsFromValue(char value)
        {
            return Enumerable.Range(0, _boardGrid.GetLength(1))
                    .Select(x => _boardGrid[currentRow, x])
                    .Where(x => x.Value == value && x.State == BoardCellState.IncorrectPosition)
                    .ToArray().Length;
        }
    }
}
