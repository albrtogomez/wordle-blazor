using Microsoft.AspNetCore.Components;
using WordleBlazor.Models.Enums;
using WordleBlazor.Services;

namespace WordleBlazor.Components
{
    public partial class Keyboard
    {
        [CascadingParameter]
        public GameBoard? AncestorComponent { get; set; }

        private void EnterNextLetter(char value)
        {
            GameManagerService.EnterNextValue(value);
            AncestorComponent?.NotifyChange();
        }

        private void RemoveLastLetter()
        {
            GameManagerService.RemoveLastValue();
            AncestorComponent?.NotifyChange();
        }

        private void SendAnswer()
        {
            GameManagerService.CheckCurrentLineSolution();
            AncestorComponent?.NotifyChange();
        }

        private string GetKeyStateClass(char key)
        {
            var keyStatus = GameManagerService.UsedKeys.FirstOrDefault(x => x.Key == key);

            if (keyStatus.Equals(default(KeyValuePair<char, KeyState>)))
                return "";

            return keyStatus.Value switch
            {
                KeyState.Correct => "keyboard-correct-letter",
                KeyState.IncorrectPosition => "keyboard-incorrect-position-letter",
                KeyState.Wrong => "keyboard-incorrect-letter",
                _ => ""
            };
        }
    }
}