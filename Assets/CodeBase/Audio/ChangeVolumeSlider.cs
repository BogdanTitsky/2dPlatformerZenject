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
        [SerializeField] private VolumeType type;
        [SerializeField] private Slider slider;
        
        private PlayerProgress _progress;

        private AudioMixer _audioMixer;

        [Inject]
        private void Init(IPersistentProgressService progress, AudioMixer audioMixer)
        {
            _audioMixer = audioMixer;
            _progress = progress.Progress;
            AddListeners();
        }

        private void AddListeners()
        {
            switch (type)
            {
                case VolumeType.Master:
                    slider.onValueChanged.AddListener(SetMasterVolume);
                    slider.value = DecibelToSlider(_progress.VolumeData.Master);
                    break;
                case VolumeType.SoundFx:
                    slider.onValueChanged.AddListener(SetSoundFxVolume);
                    slider.value = DecibelToSlider(_progress.VolumeData.SoundFX);
                    break;
                case VolumeType.Music:
                    slider.onValueChanged.AddListener(SetMusicVolume);
                    slider.value = DecibelToSlider(_progress.VolumeData.Music);
                    break;
            }
        }

        public void LoadProgress(PlayerProgress progress)
        {
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            if (type == VolumeType.Master)
                progress.VolumeData.Master = SliderToDecibel();
            else if (type == VolumeType.SoundFx)
                progress.VolumeData.SoundFX = SliderToDecibel();
            else if (type == VolumeType.Music) 
                progress.VolumeData.Music = SliderToDecibel();
        }
        
        private void SetMasterVolume(float value) => _audioMixer.SetFloat(Constants.Master, SliderToDecibel());

        private void SetSoundFxVolume(float value) => _audioMixer.SetFloat(Constants.SoundFX, SliderToDecibel());
        
        private void SetMusicVolume(float value) => _audioMixer.SetFloat(Constants.Music, SliderToDecibel());

        private float SliderToDecibel() => 
            Mathf.Log10(Mathf.Clamp(slider.value, 0.0001f, 1f)) * 20;
        
        private float DecibelToSlider(float decibelValue) => 
            Mathf.Pow(10, decibelValue / 20f);
    }
}