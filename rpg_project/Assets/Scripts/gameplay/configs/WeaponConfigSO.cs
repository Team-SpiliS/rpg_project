using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Weapon")]
public class WeaponConfigSO : ScriptableObject
{
    public string weaponName;
    public int baseDamage;
    public int weaponVisualIndex; 

    [Header("Ranged Settings")]
    public GameObject projectilePrefab; 
    public float retreatDistance = 5f;
}