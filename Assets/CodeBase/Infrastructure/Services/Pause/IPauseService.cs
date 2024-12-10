using System;

namespace CodeBase.Infrastructure.Services.Pause
{
    public interface IPauseService
    {
        bool IsPaused { get; }
        void SetPaused(bool isPaused);
        event Action PauseChanged;
    }
}