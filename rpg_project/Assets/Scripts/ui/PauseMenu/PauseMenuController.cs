using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController
{
    private readonly PauseMenuView _view;
    private readonly ISaveService _saveService;
    private bool _isPaused;
    private readonly IScoreInteractor _scoreInteractor;

    public PauseMenuController(PauseMenuView view, ISaveService saveService, IScoreInteractor scoreInteractor)
    {
        _view = view;
        _saveService = saveService;
        _scoreInteractor = scoreInteractor;

        _view.OnContinueClicked += TogglePause;
        _view.OnSaveClicked += SaveGame;
        _view.OnLoadClicked += LoadGame;
        _view.OnMainMenuClicked += GoToMainMenu;
    }

    public void TogglePause()
    {
        _isPaused = !_isPaused;
        Time.timeScale = _isPaused ? 0f : 1f; 

        if (_isPaused)
        {
            if (_scoreInteractor != null)
            {
                _view.SetScoreText($"Счёт: {_scoreInteractor.CurrentScore}");
            }
            _view.Show();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            _view.Hide();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void SaveGame() => _saveService.SaveGame();

    private void LoadGame()
    {
        _saveService.LoadGame();
        TogglePause();
    }

    private void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void Dispose()
    {
        _view.OnContinueClicked -= TogglePause;
        _view.OnSaveClicked -= SaveGame;
        _view.OnLoadClicked -= LoadGame;
        _view.OnMainMenuClicked -= GoToMainMenu;
    }
}
