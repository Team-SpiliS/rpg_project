using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Factory")]
public class EnemyFactorySO : ScriptableObject
{
    public string enemyId;
    public GameObject prefab;
    public WeaponConfigSO[] weapon;
    public ElementConfigSO[] element;

    public void Spawn(Vector3 position)
    {
        CreateInstance(position);
    }

    public EnemyBase CreateInstance(Vector3 position)
    {
        GameObject instance = Instantiate(prefab, position, Quaternion.identity);
        EnemyBase enemy = instance.GetComponent<EnemyBase>();

        if (enemy != null)
        {
            enemy.weaponConfig = weapon[Random.Range(0, weapon.Length)];
            if (enemy.weaponConfig != null && element.Length > 0)
            {
                enemy.elementConfig = element[Random.Range(0, element.Length)];
            }
            enemy.originFactory = this;
            enemy.ApplyVisuals();
        }
        return enemy;
    }
}