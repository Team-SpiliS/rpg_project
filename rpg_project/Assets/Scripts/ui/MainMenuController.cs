using UnityEngine.SceneManagement;

public class MainMenuController
{
    private readonly MainMenuModel _model;
    private readonly MainMenuView _view;
    private readonly IAudioService _audioService;
    private readonly IGameSettings _gameSettings;

    public MainMenuController(MainMenuModel model, MainMenuView view, IAudioService audioService, IGameSettings settings)
    {
        _model = model;
        _view = view;
        _audioService = audioService;
        _gameSettings = settings;

        _view.OnPlayClicked += HandlePlayClicked;
        _view.OnVolumeSliderChanged += HandleVolumeChanged;
        _view.OnPeacefulToggleChanged += HandlePeacefulChanged;

        _model.OnVolumeChanged += _view.UpdateVolumeSlider;

        if (_audioService != null)
        {
            _model.MusicVolume = _audioService.GetVolume();
        }
        _model.IsPeacefulMode = _gameSettings.IsPeacefulMode;

        _view.UpdateVolumeSlider(_model.MusicVolume);
        _view.UpdatePeacefulToggle(_model.IsPeacefulMode);
    }

    private void HandlePlayClicked()
    {
        SceneManager.LoadScene("Level_1"); 
    }

    private void HandleVolumeChanged(float newVolume)
    {
        _model.MusicVolume = newVolume;

        _audioService?.SetVolume(newVolume);
    }
    private void HandlePeacefulChanged(bool isOn)
    {
        _model.IsPeacefulMode = isOn;
        _gameSettings.IsPeacefulMode = isOn;
    }
}