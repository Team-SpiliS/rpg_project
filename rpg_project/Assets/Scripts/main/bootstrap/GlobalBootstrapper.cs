using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-100)]
public class GlobalBootstrapper : MonoBehaviour
{
    [Header("Настройки Аудио")]
    [SerializeField] private AudioSource mainMusicSource;
    [SerializeField] private string nextSceneName = "MainMenu";
    [SerializeField] private EnemyKilledEventSO _enemyDeathEvent;
    private ScoreInteractor _scoreInteractor;
    private PlayerInputService _inputReader;

    private void Awake()
    {
        transform.SetParent(null);

        if (FindObjectsByType<GlobalBootstrapper>(FindObjectsSortMode.None).Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        InitializeServices();

        if (!string.IsNullOrEmpty(nextSceneName) && SceneManager.GetActiveScene().name != nextSceneName)
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }

    private void InitializeServices()
    {
        ServiceLocator.Clear();
        _scoreInteractor = new ScoreInteractor(_enemyDeathEvent);
        ServiceLocator.Register<IScoreInteractor>(_scoreInteractor);

        IAudioService audioService = new UnityAudioService(mainMusicSource);
        ServiceLocator.Register<IAudioService>(audioService);

        ISaveRepository repository = new JsonSaveRepository();
        IWorldStateApplier worldStateApplier = new WorldStateApplier();
        SaveInteractor saveInteractor = new SaveInteractor(repository, _scoreInteractor, worldStateApplier);
        ServiceLocator.Register<ISaveService>(saveInteractor);

        _inputReader = new PlayerInputService();
        ServiceLocator.Register<IPlayerInputService>(_inputReader);

        ServiceLocator.Register<IGameSettings>(new GameSettingsService());

        audioService.PlayMusic();
    }
    private void OnDestroy()
    {
        _scoreInteractor?.Dispose();
        _inputReader?.Dispose();
    }
}
