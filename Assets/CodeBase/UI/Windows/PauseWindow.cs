using CodeBase.Infrastructure.Services.Pause;
using Zenject;

namespace CodeBase.UI.Windows
{
    public class PauseWindow : WindowBase
    {
        private IPauseService _pauseService;

        [Inject]
        public void Init(IPauseService pauseService)
        {
            _pauseService = pauseService;
        }

        protected override void OnEnableWindow()
        {
            _pauseService.SetPaused(true);
        }

        protected override void OnWindowClose()
        {
            _pauseService.SetPaused(false);
        }
    }
}