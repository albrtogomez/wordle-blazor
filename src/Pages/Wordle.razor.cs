using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using WordleBlazor.Components;
using WordleBlazor.Models.Enums;
using WordleBlazor.Services;

namespace WordleBlazor.Pages
{
    public partial class Wordle
    {
        private readonly IReadOnlyList<string> validKeys = new List<string>()
        {
            "ENTER", "BACKSPACE",
            "Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P",
            "A", "S", "D", "F", "G", "H", "J", "K", "L", "Ñ",
            "Z", "X", "C", "V", "B", "N", "M"
        };

        private ElementReference mainDiv;
        private GameBoard? gameBoard;
        private bool showStats;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await mainDiv.FocusAsync();
            }
        }

        public void ShowStats()
        {
            showStats = true;
            StateHasChanged();
        }

        private void CloseStats()
        {
            showStats = false;
            StateHasChanged();
        }

        private async Task KeyDown(KeyboardEventArgs e)
        {
            string key = e.Key.ToUpper();
            if (GameManagerService.GameState == GameState.Playing && validKeys.Contains(key))
            {
                if (key == "ENTER")
                {
                    await GameManagerService.CheckCurrentLineSolution();
                }
                else if (key == "BACKSPACE")
                {
                    GameManagerService.RemoveLastValue();
                }
                else
                {
                    GameManagerService.EnterNextValue(Convert.ToChar(key));
                }

                gameBoard?.NotifyChange();
            }
        }
    }
}