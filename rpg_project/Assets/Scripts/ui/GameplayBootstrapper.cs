using UnityEngine;

public class GameplayBootstrapper : MonoBehaviour
{
    [SerializeField] private PauseMenuView _pauseMenuView;

    private PauseMenuController _pauseController;
    private ISaveService _saveService;

    private void Start()
    {
        _saveService = ServiceLocator.Get<ISaveService>();

        _pauseController = new PauseMenuController(_pauseMenuView, _saveService);

        ApplySaveIfStateExists();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _pauseController.TogglePause();
        }
    }

    private void ApplySaveIfStateExists()
    {
        var interactor = _saveService as SaveInteractor;
        WorldSnapshot data = interactor?.GetCurrentData();
        if (data == null || data.player == null)
        {
            return;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = data.player.position;
            var hc = player.GetComponent<HealthComponent>();
            if (hc != null) hc.LoadHealth(data.player.health);
        }

        GameObject[] sceneEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (var enemy in sceneEnemies)
        {
            string rootName = enemy.transform.root.name;
            EnemySnapshot savedEnemy = data.enemies.Find(e => e.id == rootName);

            if (savedEnemy != null)
            {
                enemy.transform.position = savedEnemy.position;
                var hc = enemy.GetComponent<HealthComponent>();
                if (hc == null) hc = enemy.GetComponentInChildren<HealthComponent>();

                if (hc != null) hc.LoadHealth(savedEnemy.health);
            }
            else
            {
                Destroy(enemy.transform.root.gameObject);
            }
        }
    }
}