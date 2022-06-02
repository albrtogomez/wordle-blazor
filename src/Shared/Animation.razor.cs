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

        private System.Timers.Timer _endAnimationTimer = default!;
        private string _animationStyles = "";

        protected override void OnInitialized()
        {
            _endAnimationTimer = new System.Timers.Timer(Duration) { AutoReset = false };
            _endAnimationTimer.Elapsed += EndAnimation;
        }

        public void TriggerAnimation()
        {
            _animationStyles = Name;
            StateHasChanged();
            _endAnimationTimer.Start();
        }

        private void EndAnimation(object? sender, ElapsedEventArgs e)
        {
            _animationStyles = "";
            StateHasChanged();
        }
    }
}