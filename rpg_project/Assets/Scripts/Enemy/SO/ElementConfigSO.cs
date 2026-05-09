using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Element")]
public class ElementConfigSO : ScriptableObject
{
    public string elementName;
    public Color effectColor;
    public GameObject impactVFX;
    public float damageMultiplier = 1.0f;
}