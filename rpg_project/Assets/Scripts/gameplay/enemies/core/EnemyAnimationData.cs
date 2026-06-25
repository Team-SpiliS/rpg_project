using UnityEngine;
[System.Serializable]
public struct EnemyAnimationData
{
    public string idle;
    public string chase;
    public string attackTrigger;
    public string flee;

    [Header("Босс / Уникальные")]
    public string heavyAttack;   
    public string magicCast;    
    public string taunt;   
    public string stun; 
}