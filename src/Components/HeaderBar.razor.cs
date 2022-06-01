using Microsoft.AspNetCore.Components;
using WordleBlazor.Model.Enums;
using WordleBlazor.Pages;

namespace WordleBlazor.Components
{
    public partial class HeaderBar
    {
        [Parameter, EditorRequired]
        public Wordle? AncestorComponent { get; set; }

        private readonly string englishFlagPath = "images/english.svg";
        private readonly string spanishFlagPath = "images/spanish.svg";

        private string GetCurrentLanguageFlag()
        {
            if (LocalizationService.CurrentLanguage == Language.English)
            {
                return englishFlagPath;
            }
            else
            {
                return spanishFlagPath;
            }
        }

        private async Task ChangeLanguage()
        {
            await LocalizationService.SwitchLanguage();

            NavManager.NavigateTo(NavManager.Uri, forceLoad: true);
        }
    }
}