using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace CodeBase.Audio
{
    public class BackgroundMusic : MonoBehaviour, ISavedProgressReader
    {
        private AudioMixer _audioMixer;

        [Inject]
        public void Init(AudioMixer audioMixer)
        {
            _audioMixer = audioMixer;
        }

        public void LoadProgress(PlayerProgress progress)
        {
            _audioMixer.SetFloat(Constants.Master, progress.VolumeData.Master);
            _audioMixer.SetFloat(Constants.Music, progress.VolumeData.Music);
            _audioMixer.SetFloat(Constants.SoundFX, progress.VolumeData.SoundFX);
        }
    }
}