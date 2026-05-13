using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-100)]
public class GlobalBootstrapper : MonoBehaviour
{
    [Header("Настройки Аудио")]
    [SerializeField] private AudioSource mainMusicSource;
    [SerializeField] private string nextSceneName = "MainMenu";
    [SerializeField] private EnemyKilledEventSO _enemyDeathEvent;

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

        var scoreInteractor = new ScoreService(_enemyDeathEvent);
        ServiceLocator.Register<IScoreService>(scoreInteractor);

        IAudioService audioService = new UnityAudioService(mainMusicSource);
        ServiceLocator.Register<IAudioService>(audioService);

        ISaveRepository repository = new JsonSaveRepository();
        SaveInteractor saveInteractor = new SaveInteractor(repository, scoreInteractor);
        ServiceLocator.Register<ISaveService>(saveInteractor);

        ServiceLocator.Register<IGameSettings>(new GameSettingsService());

        audioService.PlayMusic();
    }
}