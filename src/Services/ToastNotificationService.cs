namespace WordleBlazor.Services
{
    public class ToastNotificationService : IDisposable
    {
        public event Action<string> OnShow = null!;
        public event Action OnHide = null!;

        private System.Timers.Timer _countdown = null!;

        public void ShowToast(string message)
        {
            OnShow?.Invoke(message);
            StartCountdown();
        }

        private void StartCountdown()
        {
            if (_countdown == null)
            {
                _countdown = new System.Timers.Timer(2900);
                _countdown.Elapsed += HideToast;
                _countdown.AutoReset = false;
            }

            if (!_countdown.Enabled)
            {
                _countdown.Start();
            }
        }

        private void HideToast(object? source, System.Timers.ElapsedEventArgs args)
        {
            OnHide?.Invoke();
        }

        public void Dispose() => _countdown?.Dispose();
    }
}
