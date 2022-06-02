using Microsoft.AspNetCore.Components;
using WordleBlazor.Model.Enums;
using WordleBlazor.Pages;

namespace WordleBlazor.Components
{
    public partial class HeaderBar
    {
        [Parameter, EditorRequired]
        public Wordle? AncestorComponent { get; set; }

        private string GetCurrentLanguageFlagPath()
        {
            if (LocalizationService.CurrentLanguage == Language.English)
            {
                return "images/english.svg";
            }
            else
            {
                return "images/spanish.svg";
            }
        }

        private async Task ChangeLanguage()
        {
            await LocalizationService.SwitchLanguage();

            NavManager.NavigateTo(NavManager.Uri, forceLoad: true);
        }
    }
}