using Microsoft.AspNetCore.Components;

namespace WordleBlazor.Components
{
    public partial class Keyboard
    {
        [CascadingParameter]
        public GameBoard? AncestorComponent { get; set; }
    }
}