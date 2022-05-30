using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using WordleBlazor.Pages;

namespace WordleBlazor.Components
{
    public partial class HeaderBar
    {
        [Inject]
        public ILocalStorageService LocalStorage { get; set; } = null!;

        [Inject]
        public NavigationManager NavManager { get; set; } = null!;

        [Parameter, EditorRequired]
        public Wordle? AncestorComponent { get; set; }

        private string currentCulture = "";

        private readonly string englishFlagPath = "images/english.svg";
        private readonly string spanishFlagPath = "images/spanish.svg";

        protected override async Task OnInitializedAsync()
        {
            currentCulture = await LocalStorage.GetItemAsync<string>("CurrentCulture");
        }

        private string GetCurrentCultureFlag()
        {
            if (currentCulture.StartsWith("es"))
            {
                return spanishFlagPath;
            }
            else
            {
                return englishFlagPath;
            }
        }

        private async Task ChangeLanguage()
        {
            var currentCulture = await LocalStorage.GetItemAsync<string>("CurrentCulture");

            if (currentCulture.StartsWith("es"))
            {
                await LocalStorage.SetItemAsync("CurrentCulture", "en-US");
            }
            else
            {
                await LocalStorage.SetItemAsync("CurrentCulture", "es-ES");
            }

            NavManager.NavigateTo(NavManager.Uri, forceLoad: true);
        }
    }
}