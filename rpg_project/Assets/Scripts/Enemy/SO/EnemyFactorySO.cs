using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Factory")]
public class EnemyFactorySO : ScriptableObject
{
    public string enemyId;
    public GameObject prefab;
    public WeaponConfigSO weapon;
    public ElementConfigSO element;

    public void Spawn(Vector3 position)
    {
        GameObject instance = Instantiate(prefab, position, Quaternion.identity);

        EnemyBase enemy = instance.GetComponent<EnemyBase>();
        if (enemy != null)
        {
            enemy.weaponConfig = weapon;
            enemy.elementConfig = element;
            enemy.originFactory = this;

            enemy.ApplyVisuals();
        }
    }

    public EnemyBase CreateInstance(Vector3 position)
    {
        GameObject instance = Instantiate(prefab, position, Quaternion.identity);
        EnemyBase enemy = instance.GetComponent<EnemyBase>();

        if (enemy != null)
        {
            enemy.weaponConfig = weapon;
            enemy.elementConfig = element;
            enemy.ApplyVisuals();
        }
        return enemy;
    }
}