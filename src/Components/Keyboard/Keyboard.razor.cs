using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;

namespace WordleBlazor.Components
{
    public partial class Keyboard
    {
        [CascadingParameter]
        public GameBoard? AncestorComponent { get; set; }

        [Inject]
        public ILocalStorageService LocalStorage { get; set; } = null!;

        private bool isSpanishKeyboard;

        protected override async Task OnInitializedAsync()
        {
            var currentCulture = await LocalStorage.GetItemAsync<string>("CurrentCulture");

            if (currentCulture?.StartsWith("es") == true)
                isSpanishKeyboard = true;
            else
                isSpanishKeyboard = false;
        }
    }
}