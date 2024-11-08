using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.Audio
{
    public class ChangeVolumeSlider : MonoBehaviour, ISavedProgress
    {
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private VolumeType type;
        [SerializeField] private Slider slider;
        
        private const string Master = "Master";
        private const string Music = "Music";
        private const string SoundFX = "SoundFX";

        private PlayerProgress _progress;

        [Inject]
        private void Init(IPersistentProgressService progress)
        {
            _progress = progress.Progress;
            AddListeners();
        }

        private void AddListeners()
        {
            switch (type)
            {
                case VolumeType.Master:
                    slider.onValueChanged.AddListener(SetMasterVolume);
                    slider.value = _progress.VolumeData.Master;
                    break;
                case VolumeType.SoundFx:
                    slider.onValueChanged.AddListener(SetSoundFxVolume);
                    slider.value = _progress.VolumeData.SoundFX;
                    break;
                case VolumeType.Music:
                    slider.onValueChanged.AddListener(SetMusicVolume);
                    slider.value = _progress.VolumeData.Music;
                    break;
            }
        }

        public void LoadProgress(PlayerProgress progress)
        {
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            if (type == VolumeType.Master)
                progress.VolumeData.Master = slider.value;
            else if (type == VolumeType.SoundFx)
                progress.VolumeData.SoundFX = slider.value;
            else if (type == VolumeType.Music) 
                progress.VolumeData.Music = slider.value;
        }
        
        private void SetMasterVolume(float value) => audioMixer.SetFloat(Master, value);

        private void SetSoundFxVolume(float value) => audioMixer.SetFloat(Music, value);
        
        private void SetMusicVolume(float value) => audioMixer.SetFloat(SoundFX, value);
        
    }
}