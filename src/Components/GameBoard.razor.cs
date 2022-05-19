using BlazorComponentUtilities;
using WordleBlazor.Models.Enums;

namespace WordleBlazor.Components
{
    public partial class GameBoard
    {
        private string? NextWordClasses => new CssBuilder()
            .AddClass("flex", ShowNextWord)
             .AddClass("hidden", !ShowNextWord)
            .Build();

        private string? KeyboardContainerClasses => new CssBuilder()
            .AddClass("flex", ShowKeyboard)
            .AddClass("hidden", !ShowKeyboard)
            .Build();

        private bool ShowNextWord => GameManagerService.GameState == GameState.Win ||
                GameManagerService.GameState == GameState.GameOver;

        private bool ShowKeyboard => GameManagerService.GameState == GameState.NotStarted ||
                GameManagerService.GameState == GameState.Playing;

        private BoardLine boardLine0 = null!;
        private BoardLine boardLine1 = null!;
        private BoardLine boardLine2 = null!;
        private BoardLine boardLine3 = null!;
        private BoardLine boardLine4 = null!;
        private BoardLine boardLine5 = null!;

        protected override async Task OnInitializedAsync()
        {
            await GameManagerService.LoadJsonSpanishDictionary();
            await GameManagerService.GetTodaySolution();
            GameManagerService.StartGame();
        }

        public void NotifyChange()
        {
            InvokeAsync(StateHasChanged);
        }

        public void TriggerShakeLineAnimation(int currentLine)
        {
            switch (currentLine)
            {
                case 0:
                    boardLine0.TriggerAnimation();
                    break;
                case 1:
                    boardLine1.TriggerAnimation();
                    break;
                case 2:
                    boardLine2.TriggerAnimation();
                    break;
                case 3:
                    boardLine3.TriggerAnimation();
                    break;
                case 4:
                    boardLine4.TriggerAnimation();
                    break;
                case 5:
                    boardLine5.TriggerAnimation();
                    break;
                default:
                    break;
            }
        }

        private string GetNextWordMessage()
        {
            if (GameManagerService.GameState == GameState.Win)
                return "Enhorabuena, has ganado!";
            else
                return "Una lástima, has perdido!";
        }

        private void PlayAgain()
        {
            GameManagerService.Reset();
        }
    }
}