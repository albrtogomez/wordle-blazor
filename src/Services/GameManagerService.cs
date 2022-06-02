using System.Net.Http.Json;
using System.Text;
using WordleBlazor.Model;
using WordleBlazor.Model.Enums;

namespace WordleBlazor.Services
{
    public class GameManagerService
    {
        private BoardCell[,] _boardGrid;
        public BoardCell[,] BoardGrid { get => _boardGrid; }

        private string _solution = "";
        public string Solution { get => _solution; }

        private Dictionary<char, KeyState> _usedKeys;
        public Dictionary<char, KeyState> UsedKeys { get => _usedKeys; }

        public GameState _gameState;
        public GameState GameState { get => _gameState; }

        public static readonly int RowSize = 6;
        public static readonly int ColumnSize = 5;

        public event Action<int> OnBoardLineWrongSolution = default!;
        public event Action<int> OnCurrentLineCheckedSolution = default!;

        private readonly HttpClient _httpClient;
        private readonly ToastNotificationService _toastNotificationService;
        private readonly BrowserLocalStorageService _localStorageService;
        private readonly LocalizationService _localizationService;

        private List<string> _validWords = new();
        private int _currentRow;
        private int _currentColumn;

        public GameManagerService(HttpClient httpClient, ToastNotificationService toastNotificationService, BrowserLocalStorageService localStorage, LocalizationService loc)
        {
            _httpClient = httpClient;
            _toastNotificationService = toastNotificationService;
            _localStorageService = localStorage;
            _localizationService = loc;
            _boardGrid = new BoardCell[RowSize, ColumnSize];
            _usedKeys = new Dictionary<char, KeyState>();

            PopulateBoard();
        }

        public async Task InitializeAndLoadData()
        {
            await _localizationService.GetCurrentCulture();
            await LoadDictionary();
            await GetTodaySolution();
        }

        public async Task StartGame()
        {
            _gameState = GameState.Playing;
            
            var storedWords = await _localStorageService.LoadGameStateFromLocalStorage();
            if (storedWords != null)
                await SetBoardGridWords(storedWords);
        }

        public void Reset()
        {
            _boardGrid = new BoardCell[RowSize, ColumnSize];
            _usedKeys = new Dictionary<char, KeyState>();

            PopulateBoard();

            _currentRow = 0;
            _currentColumn = 0;

            _gameState = GameState.Playing;
        }

        public void EnterNextValue(char value)
        {
            if (GameState == GameState.Playing)
            {
                if (_currentColumn == ColumnSize - 1 && BoardGrid[_currentRow, _currentColumn].Value != null)
                    return;

                BoardGrid[_currentRow, _currentColumn].Value = value;
                BoardGrid[_currentRow, _currentColumn].State = BoardCellState.Typing;

                if (_currentColumn < ColumnSize - 1)
                    _currentColumn++;
            }
        }

        public void RemoveLastValue()
        {
            if (GameState == GameState.Playing)
            {
                if (_currentColumn == 0 && BoardGrid[_currentRow, _currentColumn].Value == null)
                    return;

                if (_currentColumn <= ColumnSize - 1 && BoardGrid[_currentRow, _currentColumn].Value == null)
                {
                    BoardGrid[_currentRow, _currentColumn - 1].Value = null;
                    BoardGrid[_currentRow, _currentColumn - 1].State = BoardCellState.Empty;
                    _currentColumn--;
                }
                else
                {
                    BoardGrid[_currentRow, _currentColumn].Value = null;
                    BoardGrid[_currentRow, _currentColumn].State = BoardCellState.Empty;
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
                    currentLineBuilder.Append(BoardGrid[_currentRow, i].Value);
                }

                string currentLine = currentLineBuilder.ToString();

                if (currentLine.Length != _solution.Length)
                {
                    _toastNotificationService.ShowToast(_localizationService["GameManagerNotEnoughLetters"]);
                    OnBoardLineWrongSolution.Invoke(_currentRow);
                    return;
                }

                if (!_validWords.Contains(currentLine) && currentLine != _solution)
                {
                    _toastNotificationService.ShowToast(_localizationService["GameManagerWordDoesNotExist"]);
                    OnBoardLineWrongSolution.Invoke(_currentRow);
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
                            _boardGrid[_currentRow, i].State = BoardCellState.Correct;

                            if (!_usedKeys.TryAdd(currentLine[i], KeyState.Correct))
                                _usedKeys[currentLine[i]] = KeyState.Correct;
                        }
                        else
                        {
                            if (foundIndexes.Count > GetCurrentRowCorrectCellsFromValue(foundIndexes, currentLine[i]))
                            {
                                if (foundIndexes.Count > GetCurrentRowIncorrectPositionCellsFromValue(currentLine[i]))
                                {
                                    _boardGrid[_currentRow, i].State = BoardCellState.IncorrectPosition;
                                }
                                else
                                {
                                    _boardGrid[_currentRow, i].State = BoardCellState.Wrong;
                                }
                            }
                            else
                            {
                                _boardGrid[_currentRow, i].State = BoardCellState.Wrong;
                            }

                            _usedKeys.TryAdd(currentLine[i], KeyState.IncorrectPosition);
                        }
                    }
                    else
                    {
                        _boardGrid[_currentRow, i].State = BoardCellState.Wrong;
                        _usedKeys.TryAdd(currentLine[i], KeyState.Wrong);
                    }
                }

                OnCurrentLineCheckedSolution.Invoke(_currentRow);

                if (currentLine == _solution)
                {
                    _gameState = GameState.Win;
                }
                else if (_currentRow < RowSize - 1)
                {
                    _currentRow++;
                    _currentColumn = 0;
                }
                else
                {
                    _gameState = GameState.GameOver;
                }

                await _localStorageService.SaveCurrentBoardToLocalStorage(GetBoardGridWords());

                if (_gameState == GameState.Win || _gameState == GameState.GameOver)
                {
                    await _localStorageService.UpdateGameStats(_gameState, _currentRow);
                    await _localStorageService.SaveLastGameFinishedDate();
                }
            }
        }

        public int GetCurrentRow()
        {
            return _currentRow;
        }

        private async Task LoadDictionary()
        {
            string dictionaryPath;

            if (_localizationService.CurrentLanguage == Language.English)
            {
                dictionaryPath = "data/english-words.json";
            }
            else
            {
                dictionaryPath = "data/spanish-words.json";
            }

            var wordList = await _httpClient.GetFromJsonAsync<List<string>>(dictionaryPath);

            _validWords = wordList ?? new List<string>();
        }

        private async Task GetTodaySolution()
        {
            string solutionPath;

            if (_localizationService.CurrentLanguage == Language.English)
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
                _localStorageService.GameStarted = currentTime;
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

        private int GetCurrentRowCorrectCellsFromValue(List<int> foundIndexes, char value)
        {
            var valueCells = Enumerable.Range(0, _boardGrid.GetLength(1))
                .Select(x => _boardGrid[_currentRow, x])
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
                    .Select(x => _boardGrid[_currentRow, x])
                    .Where(x => x.Value == value && x.State == BoardCellState.IncorrectPosition)
                    .ToArray().Length;
        }


        private List<string> GetBoardGridWords()
        {
            var wordList = new List<string>();

            for (int i = 0; i <= _currentRow; i++)
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
                    _boardGrid[_currentRow, col].State = BoardCellState.Typing;
                    _boardGrid[_currentRow, col].Value = c;

                    col++;
                }

                await CheckCurrentLineSolution();
            }
        }
    }
}
