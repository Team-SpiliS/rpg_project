using UnityEngine;

public class UnityAudioService : IAudioService
{
    private AudioSource _musicSource;
    private float _currentVolume = 1f;

    public UnityAudioService(AudioSource musicSource)
    {
        _musicSource = musicSource;
    }

    public void SetVolume(float volume)
    {
        _currentVolume = Mathf.Clamp01(volume);
        if (_musicSource != null)
        {
            _musicSource.volume = _currentVolume;
        }
    }

    public float GetVolume() => _currentVolume;

    public void PlayMusic()
    {
        if (_musicSource != null && !_musicSource.isPlaying)
        {
            _musicSource.Play();
        }
    }
}