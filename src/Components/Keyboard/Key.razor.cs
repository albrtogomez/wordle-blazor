using Microsoft.AspNetCore.Components;
using BlazorComponentUtilities;
using WordleBlazor.Models.Enums;
using WordleBlazor.Services;

namespace WordleBlazor.Components
{
    public partial class Key
    {
        [Parameter]
        public char KeyValue { get; set; }

        [Parameter]
        public KeyType KeyType { get; set; }

        [CascadingParameter]
        public GameBoard? AncestorComponent { get; set; }

        private string KeyClasses => new CssBuilder()
            .AddClass("bg-middlegray h-[52px] cursor-pointer")
            .AddClass("flex justify-center items-center")
            .AddClass("text-center font-bold text-[15px]")
            .AddClass("rounded-md border-transparent")
            .AddClass("transition-keyactive duration-150")
            .AddClass("hover:bg-keyhober active:bg-keyactive active:border-[3px] active:border-keyactiveborder")
            .AddClass("w-[44px]", KeyType == KeyType.Letter)
            .AddClass("w-[66px]", KeyType != KeyType.Letter).Build();

        private string GetKeyStateClass()
        {
            var keyStatus = GameManagerService.UsedKeys.FirstOrDefault(x => x.Key == KeyValue);

            if (keyStatus.Equals(default(KeyValuePair<char, KeyState>)))
            {
                return "";
            }

            return keyStatus.Value switch
            {
                KeyState.Correct => "bg-greenkey border-green text-white",
                KeyState.IncorrectPosition => "bg-yellow border-yellow text-white",
                KeyState.Wrong => "bg-darkgray border-darkgray text-white",
                _ => ""
            };
        }

        private void KeyPressed()
        {
            if (KeyType == KeyType.Letter)
            {
                GameManagerService.EnterNextValue(KeyValue);
            }
            else if (KeyType == KeyType.Send)
            {
                var result = GameManagerService.CheckCurrentLineSolution();

                if (result == CheckLineResult.NotEnoughLetters || result == CheckLineResult.WordDoesntExist)
                    AncestorComponent?.TriggerShakeLineAnimation(GameManagerService.GetCurrentRow());
            }
            else if (KeyType == KeyType.Remove)
            {
                GameManagerService.RemoveLastValue();
            }

            AncestorComponent?.NotifyChange();
        }

        private string GetKeyText()
        {
            if (KeyType == KeyType.Letter)
            {
                return KeyValue.ToString();
            }
            else if (KeyType == KeyType.Send)
            {
                return "ENVIAR";
            }
            else if (KeyType == KeyType.Remove)
            {
                return "BORRAR";
            }
            else
            {
                return "";
            }
        }
    }
}