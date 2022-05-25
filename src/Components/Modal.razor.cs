using BlazorComponentUtilities;
using Microsoft.AspNetCore.Components;

namespace WordleBlazor.Components
{
    public partial class Modal : BaseComponent
    {
        [Parameter, EditorRequired]
        public RenderFragment ChildContent { get; set; } = default!;

        [Parameter, EditorRequired]
        public EventCallback OnCloseCallback { get; set; }

        private string ModalClasses => new CssBuilder()
            .AddClass(Class, !string.IsNullOrWhiteSpace(Class))
            .Build();
    }
}