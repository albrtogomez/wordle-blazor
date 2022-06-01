using BlazorComponentUtilities;
using Microsoft.AspNetCore.Components;

namespace WordleBlazor.Shared
{
    public partial class Button : BaseComponent
    {
        [Parameter]
        public string? Text { get; set; }

        [Parameter]
        public string? Icon { get; set; }

        [Parameter]
        public EventCallback OnClick { get; set; }

        private string ButtonClasses => new CssBuilder()
            .AddClass("button")
            .AddClass(Class, !string.IsNullOrWhiteSpace(Class))
            .Build();
    }
}