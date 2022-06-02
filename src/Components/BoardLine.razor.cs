using Microsoft.AspNetCore.Components;
using WordleBlazor.Shared;
using WordleBlazor.Services;

namespace WordleBlazor.Components
{
    public partial class BoardLine
    {
        [Parameter, EditorRequired]
        public int RowIndex { get; set; }

        [Parameter, EditorRequired]
        public RenderFragment ChildContent { get; set; } = default !;

        private Animation _animationRef = default !;

        protected override void OnInitialized()
        {
            GameManagerService.OnBoardLineWrongSolution += TriggerAnimation;
        }

        public void Dispose()
        {
            GameManagerService.OnBoardLineWrongSolution -= TriggerAnimation;
        }

        private void TriggerAnimation(int currentRow)
        {
            if (currentRow == RowIndex)
                _animationRef.TriggerAnimation();
        }
    }
}