using UnityEngine.SceneManagement;

public class MainMenuController
{
    private readonly MainMenuModel _model;
    private readonly MainMenuView _view;
    private readonly IAudioService _audioService; 

    public MainMenuController(MainMenuModel model, MainMenuView view, IAudioService audioService)
    {
        _model = model;
        _view = view;
        _audioService = audioService;

        _view.OnPlayClicked += HandlePlayClicked;
        _view.OnVolumeSliderChanged += HandleVolumeChanged;

        _model.OnVolumeChanged += _view.UpdateVolumeSlider;

        if (_audioService != null)
        {
            _model.MusicVolume = _audioService.GetVolume();
        }
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
}