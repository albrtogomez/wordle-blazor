using Microsoft.AspNetCore.Components;
using System.Timers;

namespace WordleBlazor.Shared
{
    public partial class Animation
    {
        [Parameter, EditorRequired]
        public string Name { get; set; } = "";

        [Parameter, EditorRequired]
        public int Duration { get; set; }

        [Parameter, EditorRequired]
        public RenderFragment ChildContent { get; set; } = default!;

        private System.Timers.Timer endAnimationTimer = default!;
        private string animationStyles = "";

        protected override void OnInitialized()
        {
            endAnimationTimer = new System.Timers.Timer(Duration) { AutoReset = false };
            endAnimationTimer.Elapsed += EndAnimation;
        }

        public void TriggerAnimation()
        {
            animationStyles = Name;
            StateHasChanged();
            endAnimationTimer.Start();
        }

        private void EndAnimation(object? sender, ElapsedEventArgs e)
        {
            animationStyles = "";
            StateHasChanged();
        }
    }
}