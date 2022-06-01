using Blazored.LocalStorage;
using System.Net.Http.Json;
using System.Text;
using WordleBlazor.Model;
using WordleBlazor.Model.Enums;

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

        private string _solution = "";
        public string Solution { get => _solution; }

        private Dictionary<char, KeyState> _usedKeys;
        public Dictionary<char, KeyState> UsedKeys { get => _usedKeys; }

        public GameState _gameState;
        public GameState GameState { get => _gameState; }

        private readonly HttpClient _httpClient;
        private readonly ToastNotificationService _toastNotificationService;
        private readonly ILocalStorageService _localStorage;
        private readonly LocalizationService _loc;

        private DateTime gameStarted;
        private DateTime lastGamePlayedDate;
        private List<string> validWords = new();
        private int currentRow;
        private int currentColumn;

        public GameManagerService(HttpClient httpClient, ToastNotificationService toastNotificationService, ILocalStorageService localStorage, LocalizationService loc)
        {
            _httpClient = httpClient;
            _toastNotificationService = toastNotificationService;
            _localStorage = localStorage;
            _loc = loc;
            _boardGrid = new BoardCell[RowSize, ColumnSize];
            _usedKeys = new Dictionary<char, KeyState>();

            PopulateBoard();
        }

        public async Task LoadGameData()
        {
            await _loc.GetCurrentCulture();
            await LoadDictionary();
            await GetTodaySolution();
        }

        private async Task LoadDictionary()
        {
            string dictionaryPath;

            if (_loc.CurrentLanguage == Language.English)
            {
                dictionaryPath = "data/english-words.json";
            }
            else
            {
                dictionaryPath = "data/spanish-words.json";
            }

            var wordList = await _httpClient.GetFromJsonAsync<List<string>>(dictionaryPath);

            validWords = wordList ?? new List<string>();
        }

        private async Task GetTodaySolution()
        {
            string solutionPath;

            if (_loc.CurrentLanguage == Language.English)
            {
                solutionPath = "data/daily-solutions-en.json";
            }
            else
            {
                solutionPath = "data/daily-solutions-sp.json";
            }

            var solutions = await _httpClient.GetFromJsonAsync<Dictionary<int, string>>(solutionPath);

            if (solutions != null)
            {
                var currentTime = DateTime.Now;
                _solution = solutions[currentTime.DayOfYear];
                gameStarted = currentTime;
            }
        }

        public async Task StartGame()
        {
            _gameState = GameState.Playing;
            await LoadGameStateFromLocalStorage();
        }

        public void Reset()
        {
            _boardGrid = new BoardCell[RowSize, ColumnSize];
            _usedKeys = new Dictionary<char, KeyState>();

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

        public async Task CheckCurrentLineSolution()
        {
            if (GameState == GameState.Playing)
            {
                StringBuilder currentLineBuilder = new();

                for (int i = 0; i < ColumnSize; i++)
                {
                    currentLineBuilder.Append(BoardGrid[currentRow, i].Value);
                }

                string currentLine = currentLineBuilder.ToString();

                if (currentLine.Length != _solution.Length)
                {
                    _toastNotificationService.ShowToast(_loc["GameManagerNotEnoughLetters"]);
                    OnBoardLineWrongSolution.Invoke(currentRow);
                    return;
                }

                if (!validWords.Contains(currentLine) && currentLine != _solution)
                {
                    _toastNotificationService.ShowToast(_loc["GameManagerWordDoesNotExist"]);
                    OnBoardLineWrongSolution.Invoke(currentRow);
                    return;
                }

                for (int i = 0; i < currentLine.Length; i++)
                {
                    var foundIndexes = new List<int>();

                    for (int j = 0; j < _solution.Length; j++)
                    {
                        if (_solution[j] == currentLine[i])
                            foundIndexes.Add(j);
                    }

                    if (foundIndexes.Count > 0)
                    {
                        if (foundIndexes.Contains(i))
                        {
                            _boardGrid[currentRow, i].State = BoardCellState.Correct;

                            if (!_usedKeys.TryAdd(currentLine[i], KeyState.Correct))
                                _usedKeys[currentLine[i]] = KeyState.Correct;
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

                            _usedKeys.TryAdd(currentLine[i], KeyState.IncorrectPosition);
                        }
                    }
                    else
                    {
                        _boardGrid[currentRow, i].State = BoardCellState.Wrong;
                        _usedKeys.TryAdd(currentLine[i], KeyState.Wrong);
                    }
                }

                OnCurrentLineCheckedSolution.Invoke(currentRow);

                if (currentLine == _solution)
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

                await SaveCurrentGameStateToLocalStorage();
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

        private async Task LoadGameStateFromLocalStorage()
        {
            DateTime localStorageLastDayPlayed = await _localStorage.GetItemAsync<DateTime>(nameof(lastGamePlayedDate) + _loc.GetCurrentLanguageSuffix());
            var today = DateTime.Now.Date;

            if (localStorageLastDayPlayed == today)
            {
                lastGamePlayedDate = localStorageLastDayPlayed;

                var board = await _localStorage.GetItemAsync<List<string>>(nameof(BoardGrid) + _loc.GetCurrentLanguageSuffix());
                if (board != null)
                {
                    await SetBoardGridWords(board);
                }
            }
            else
            {
                lastGamePlayedDate = gameStarted.Date;
                await _localStorage.SetItemAsync(nameof(lastGamePlayedDate) + _loc.GetCurrentLanguageSuffix(), gameStarted.Date);
                await _localStorage.RemoveItemAsync(nameof(BoardGrid) + _loc.GetCurrentLanguageSuffix());
            }
        }

        private async Task SaveCurrentGameStateToLocalStorage()
        {
            await _localStorage.SetItemAsync(nameof(BoardGrid) + _loc.GetCurrentLanguageSuffix(), GetBoardGridWords());

            if (_gameState == GameState.Win || _gameState == GameState.GameOver)
            {
                await UpdateGameStats();
                await _localStorage.SetItemAsync("lastGameFinishedDate" + _loc.GetCurrentLanguageSuffix(), lastGamePlayedDate);
            }
        }

        private async Task UpdateGameStats()
        {
            var lastGameFinishedDate = await _localStorage.GetItemAsync<DateTime>("lastGameFinishedDate" + _loc.GetCurrentLanguageSuffix());
            var today = DateTime.Now.Date;

            if (lastGameFinishedDate != today)
            {
                var stats = await _localStorage.GetItemAsync<Stats>(nameof(Stats) + _loc.GetCurrentLanguageSuffix());
                if (stats == null)
                {
                    stats = new Stats();
                }

                stats.GamesPlayed++;

                if (GameState == GameState.Win)
                {
                    stats.GamesWon++;
                    stats.CurrentStreak++;

                    if (stats.CurrentStreak > stats.BestStreak)
                        stats.BestStreak = stats.CurrentStreak;

                    stats.GamesResultDistribution[currentRow + 1]++;
                }
                else
                {
                    stats.CurrentStreak = 0;
                    stats.GamesResultDistribution[-1]++;
                }

                await _localStorage.SetItemAsync(nameof(Stats) + _loc.GetCurrentLanguageSuffix(), stats);
            }
        }

        private List<string> GetBoardGridWords()
        {
            var wordList = new List<string>();

            for (int i = 0; i <= currentRow; i++)
            {
                if (_boardGrid[i, 0].State != BoardCellState.Empty && _boardGrid[i, 0].State != BoardCellState.Typing)
                {
                    string word = "";

                    for (int j = 0; j < ColumnSize; j++)
                    {
                        word += _boardGrid[i, j].Value;
                    }

                    wordList.Add(word);
                }
            }

            return wordList;
        }

        private async Task SetBoardGridWords(List<string> words)
        {
            foreach (var word in words)
            {
                int col = 0;

                foreach (var c in word)
                {
                    _boardGrid[currentRow, col].State = BoardCellState.Typing;
                    _boardGrid[currentRow, col].Value = c;

                    col++;
                }

                await CheckCurrentLineSolution();
            }
        }
    }
}
