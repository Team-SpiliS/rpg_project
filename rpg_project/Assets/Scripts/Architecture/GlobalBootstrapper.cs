using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-100)]
public class GlobalBootstrapper : MonoBehaviour
{
    [Header("Настройки Аудио")]
    [SerializeField] private AudioSource mainMusicSource;
    [SerializeField] private string nextSceneName = "MainMenu"; 

    private void Awake()
    {
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

        IAudioService audioService = new UnityAudioService(mainMusicSource);
        ServiceLocator.Register<IAudioService>(audioService);

        ISaveRepository repository = new JsonSaveRepository();
        SaveInteractor saveInteractor = new SaveInteractor(repository);
        ServiceLocator.Register<ISaveService>(saveInteractor); 

        audioService.PlayMusic();
    }
}