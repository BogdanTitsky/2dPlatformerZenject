using System;

namespace CodeBase.Infrastructure.Services.Pause
{
    public class PauseService : IPauseService
    {
        private bool _isPaused;
        public event Action PauseChanged;

        public bool IsPaused
        {
            get => _isPaused;
            private set
            {
                if (_isPaused == value) return;
                
                _isPaused = value;
                PauseChanged?.Invoke();
            }
        }

        public void SetPaused(bool isPaused)
        {
            IsPaused = isPaused;
        }
    }
}