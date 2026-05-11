using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController
{
    private readonly PauseMenuView _view;
    private readonly ISaveService _saveService;
    private bool _isPaused;
    private readonly IScoreService _scoreService;

    public PauseMenuController(PauseMenuView view, ISaveService saveService, IScoreService scoreService)
    {
        _view = view;
        _saveService = saveService;
        _scoreService = scoreService;

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
            if (_scoreService != null)
            {
                _view.SetScoreText($"Ñ÷¸ò: {_scoreService.CurrentScore}");
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
        var bootstrapper = Object.FindObjectOfType<GameplayBootstrapper>();
        if (bootstrapper != null)
        {
            bootstrapper.ApplySaveIfStateExists();
        }
        TogglePause();
    }

    private void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}