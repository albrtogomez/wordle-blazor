namespace WordleBlazor.Services
{
    public class ToastNotificationService : IDisposable
    {
        public event Action<string> OnShow = null!;
        public event Action OnHide = null!;

        private System.Timers.Timer countdown = null!;

        public void ShowToast(string message)
        {
            OnShow?.Invoke(message);
            StartCountdown();
        }

        private void StartCountdown()
        {
            if (countdown == null)
            {
                countdown = new System.Timers.Timer(2900);
                countdown.Elapsed += HideToast;
                countdown.AutoReset = false;
            }

            if (!countdown.Enabled)
            {
                countdown.Start();
            }
        }

        private void HideToast(object? source, System.Timers.ElapsedEventArgs args)
        {
            OnHide?.Invoke();
        }

        public void Dispose() => countdown?.Dispose();
    }
}
