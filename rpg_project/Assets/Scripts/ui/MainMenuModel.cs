using System;

public class MainMenuModel
{
    private float _musicVolume = 1f;
    private bool _isPeacefulMode = false;

    public event Action<float> OnVolumeChanged;
    public event Action<bool> OnPeacefulModeChanged;

    public float MusicVolume
    {
        get => _musicVolume;
        set
        {
            _musicVolume = value;
            OnVolumeChanged?.Invoke(_musicVolume);
        }
    }

    public bool IsPeacefulMode
    {
        get => _isPeacefulMode;
        set { _isPeacefulMode = value; OnPeacefulModeChanged?.Invoke(_isPeacefulMode); }
    }
}