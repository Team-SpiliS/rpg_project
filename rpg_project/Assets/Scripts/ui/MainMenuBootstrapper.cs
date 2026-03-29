using UnityEngine;

public class MainMenuBootstrapper : MonoBehaviour
{
    [SerializeField] private MainMenuView _mainMenuView;

    private MainMenuController _controller;

    private void Start()
    {
        IAudioService audioService = ServiceLocator.Get<IAudioService>();

        MainMenuModel model = new MainMenuModel();

        _controller = new MainMenuController(model, _mainMenuView, audioService);

        Debug.Log("[MainMenuBootstrapper] MVC Главного меню инициализирован.");
    }
}