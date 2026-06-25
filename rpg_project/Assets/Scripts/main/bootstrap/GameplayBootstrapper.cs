using UnityEngine;

public class GameplayBootstrapper : MonoBehaviour
{
    [SerializeField] private PauseMenuView _pauseMenuView;
    [SerializeField] private PauseMenuInputHandler _pauseInputHandler;

    private PauseMenuController _pauseController;

    private void Start()
    {
        var saveService = ServiceLocator.Get<ISaveService>();
        var scoreInteractor = ServiceLocator.Get<IScoreInteractor>();

        _pauseController = new PauseMenuController(_pauseMenuView, saveService, scoreInteractor);
        if (_pauseInputHandler != null)
        {
            _pauseInputHandler.Initialize(_pauseController);
        }
        else
        {
            Debug.LogWarning("PauseMenuInputHandler не прикреплен");
        }
    }

    private void OnDestroy()
    {
        _pauseController?.Dispose();
    }
}
