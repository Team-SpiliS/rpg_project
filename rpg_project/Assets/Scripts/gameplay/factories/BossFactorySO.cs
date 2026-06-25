using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Boss Factory")]
public class BossFactorySO : ScriptableObject
{
    [SerializeField] private string bossId = "Boss";
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private WeaponConfigSO[] bossWeapons;
    [SerializeField] private ElementConfigSO[] bossElements;

    public string BossId => bossId;

    public BossEnemy Create(Vector3 position)
    {
        if (bossPrefab == null)
        {
            return null;
        }

        GameObject bossObject = Instantiate(bossPrefab, position, Quaternion.identity);
        if (!bossObject.TryGetComponent(out BossEnemy boss))
        {
            Object.Destroy(bossObject);
            return null;
        }

        boss.weaponConfig = GetRandomItem(bossWeapons);
        boss.elementConfig = GetRandomItem(bossElements);
        boss.ApplyVisuals();
        boss.name = "BOSS_INSTANCE";

        return boss;
    }

    private static T GetRandomItem<T>(T[] items) where T : Object
    {
        if (items == null || items.Length == 0) return null;
        return items[Random.Range(0, items.Length)];
    }
}
