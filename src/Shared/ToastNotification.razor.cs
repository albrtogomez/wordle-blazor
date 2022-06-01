using WordleBlazor.Services;

namespace WordleBlazor.Shared
{
    public partial class ToastNotification
    {
        private string Message { get; set; } = "";
        private bool IsVisible { get; set; }

        protected override void OnInitialized()
        {
            ToastNotificationService.OnShow += ShowToast;
            ToastNotificationService.OnHide += HideToast;
        }

        private void ShowToast(string message)
        {
            Message = message;
            IsVisible = true;
            StateHasChanged();
        }

        private void HideToast()
        {
            IsVisible = false;
            StateHasChanged();
        }

        public void Dispose()
        {
            ToastNotificationService.OnShow -= ShowToast;
            ToastNotificationService.OnHide -= HideToast;
        }
    }
}