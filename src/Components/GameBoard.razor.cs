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

        private BoardCell boardCell00 = null!;
        private BoardCell boardCell01 = null!;
        private BoardCell boardCell02 = null!;
        private BoardCell boardCell03 = null!;
        private BoardCell boardCell04 = null!;

        private BoardCell boardCell10 = null!;
        private BoardCell boardCell11 = null!;
        private BoardCell boardCell12 = null!;
        private BoardCell boardCell13 = null!;
        private BoardCell boardCell14 = null!;

        private BoardCell boardCell20 = null!;
        private BoardCell boardCell21 = null!;
        private BoardCell boardCell22 = null!;
        private BoardCell boardCell23 = null!;
        private BoardCell boardCell24 = null!;

        private BoardCell boardCell30 = null!;
        private BoardCell boardCell31 = null!;
        private BoardCell boardCell32 = null!;
        private BoardCell boardCell33 = null!;
        private BoardCell boardCell34 = null!;

        private BoardCell boardCell40 = null!;
        private BoardCell boardCell41 = null!;
        private BoardCell boardCell42 = null!;
        private BoardCell boardCell43 = null!;
        private BoardCell boardCell44 = null!;

        private BoardCell boardCell50 = null!;
        private BoardCell boardCell51 = null!;
        private BoardCell boardCell52 = null!;
        private BoardCell boardCell53 = null!;
        private BoardCell boardCell54 = null!;

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

        public void TriggerFlipAnimation(int currentLine)
        {
            switch (currentLine)
            {
                case 0:
                    boardCell00.TriggerFlip();
                    boardCell01.TriggerFlip();
                    boardCell02.TriggerFlip();
                    boardCell03.TriggerFlip();
                    boardCell04.TriggerFlip();
                    break;
                case 1:
                    boardCell10.TriggerFlip();
                    boardCell11.TriggerFlip();
                    boardCell12.TriggerFlip();
                    boardCell13.TriggerFlip();
                    boardCell14.TriggerFlip();
                    break;
                case 2:
                    boardCell20.TriggerFlip();
                    boardCell21.TriggerFlip();
                    boardCell22.TriggerFlip();
                    boardCell23.TriggerFlip();
                    boardCell24.TriggerFlip();
                    break;
                case 3:
                    boardCell30.TriggerFlip();
                    boardCell31.TriggerFlip();
                    boardCell32.TriggerFlip();
                    boardCell33.TriggerFlip();
                    boardCell34.TriggerFlip();
                    break;
                case 4:
                    boardCell40.TriggerFlip();
                    boardCell41.TriggerFlip();
                    boardCell42.TriggerFlip();
                    boardCell43.TriggerFlip();
                    boardCell44.TriggerFlip();
                    break;
                case 5:
                    boardCell50.TriggerFlip();
                    boardCell51.TriggerFlip();
                    boardCell52.TriggerFlip();
                    boardCell53.TriggerFlip();
                    boardCell54.TriggerFlip();
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