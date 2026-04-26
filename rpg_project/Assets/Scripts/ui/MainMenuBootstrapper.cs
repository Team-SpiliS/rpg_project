using UnityEngine;

public class MainMenuBootstrapper : MonoBehaviour
{
    [SerializeField] private MainMenuView _mainMenuView;

    private MainMenuController _controller;

    private void Start()
    {
        IAudioService audioService = ServiceLocator.Get<IAudioService>();
        IGameSettings settings = ServiceLocator.Get<IGameSettings>();

        MainMenuModel model = new MainMenuModel();

        _controller = new MainMenuController(model, _mainMenuView, audioService, settings);
    }
}