using System.Collections.Generic;
using CodeBase.Audio;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;
using UnityEngine;
using Zenject;

namespace CodeBase.UI.Windows
{

    public class SettingsWindow : WindowBase
    {
        [SerializeField] private ChangeVolumeSlider[] sliders;
        private readonly List<ISavedProgress> progressesReaders =  new();
        
        private ISaveLoadService _saveLoadService;

        [Inject]
        public void Init(ISaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;
            
            foreach (ChangeVolumeSlider i in sliders)
            {
                progressesReaders.Add(i);
            }
            
            closeButton.onClick.AddListener(SaveProgress);
        }

        private void SaveProgress()
        {
            _saveLoadService.SaveProgress(progressesReaders);
        }
    }
}