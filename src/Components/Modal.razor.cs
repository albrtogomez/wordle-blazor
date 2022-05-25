using BlazorComponentUtilities;
using Microsoft.AspNetCore.Components;

namespace WordleBlazor.Components
{
    public partial class Modal : BaseComponent
    {
        [Parameter, EditorRequired]
        public RenderFragment ChildContent { get; set; } = default!;

        [Parameter]
        public bool Visible { get; set; } = true;

        private string ModalClasses => new CssBuilder()
            .AddClass("visible", Visible)
            .AddClass("hidden", !Visible)
            .AddClass(Class, !string.IsNullOrWhiteSpace(Class))
            .Build();
    }
}