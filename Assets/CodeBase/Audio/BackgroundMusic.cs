using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.Audio;

namespace CodeBase.Audio
{
    public class BackgroundMusic : MonoBehaviour, ISavedProgressReader
    {
        [SerializeField] private AudioMixer audioMixer;
        
        public void LoadProgress(PlayerProgress progress)
        {
            audioMixer.SetFloat(Constants.Master, progress.VolumeData.Master);
            audioMixer.SetFloat(Constants.Music, progress.VolumeData.Music);
            audioMixer.SetFloat(Constants.SoundFX, progress.VolumeData.SoundFX);
        }
    }
}