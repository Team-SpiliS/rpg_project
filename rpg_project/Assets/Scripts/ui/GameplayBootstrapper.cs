using UnityEngine;

public class GameplayBootstrapper : MonoBehaviour
{
    [SerializeField] private PauseMenuView _pauseMenuView;

    private PauseMenuController _pauseController;
    private ISaveService _saveService;

    private void Start()
    {
        _saveService = ServiceLocator.Get<ISaveService>();
        var scoreService = ServiceLocator.Get<IScoreService>();

        _pauseController = new PauseMenuController(_pauseMenuView, _saveService, scoreService);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _pauseController.TogglePause();
        }
    }

    public void ApplySaveIfStateExists()
    {
        WorldSnapshot data = _saveService.GetCurrentData();
        if (data == null) return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && data.player != null)
        {
            var cc = player.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;

            player.transform.position = data.player.position;

            if (cc != null) cc.enabled = true;

            var hc = player.GetComponent<HealthComponent>();
            if (hc != null) hc.LoadHealth(data.player.health);
        }

        var spawner = FindObjectOfType<UniversalSpawner>();
        if (spawner != null)
        {
            spawner.SetDeathCount(data.deathCount); 
            spawner.RestoreFromSave(data.enemies);
        }
    }
}