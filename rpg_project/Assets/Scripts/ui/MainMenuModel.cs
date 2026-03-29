using System;

public class MainMenuModel
{
    private float _musicVolume = 1f;

    public event Action<float> OnVolumeChanged;

    public float MusicVolume
    {
        get => _musicVolume;
        set
        {
            _musicVolume = value;
            OnVolumeChanged?.Invoke(_musicVolume);
        }
    }
}