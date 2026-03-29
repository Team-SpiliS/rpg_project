using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController
{
    private readonly PauseMenuView _view;
    private readonly ISaveService _saveService;
    private bool _isPaused;

    public PauseMenuController(PauseMenuView view, ISaveService saveService)
    {
        _view = view;
        _saveService = saveService;

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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }

    private void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}