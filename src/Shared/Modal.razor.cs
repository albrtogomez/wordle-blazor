using BlazorComponentUtilities;
using Microsoft.AspNetCore.Components;

namespace WordleBlazor.Shared
{
    public partial class Modal : BaseComponent
    {
        [Parameter, EditorRequired]
        public RenderFragment ChildContent { get; set; } = default!;

        [Parameter, EditorRequired]
        public EventCallback OnCloseCallback { get; set; }

        [Parameter]
        public string Title { get; set; } = null!;

        private string ModalClasses => new CssBuilder()
            .AddClass(Class, !string.IsNullOrWhiteSpace(Class))
            .Build();
    }
}