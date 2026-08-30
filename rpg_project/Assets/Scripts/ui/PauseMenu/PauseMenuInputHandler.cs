using UnityEngine;

public class PauseMenuInputHandler : MonoBehaviour
{
    private PauseMenuController _pauseController;

    public void Initialize(PauseMenuController pauseController)
    {
        _pauseController = pauseController;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _pauseController?.TogglePause();
        }
    }
}
