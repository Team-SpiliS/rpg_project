using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Enemy Killed Event")]
public class EnemyKilledEventSO : ScriptableObject
{
    public event UnityAction<EnemyBase> OnEnemyKilled;

    public void RaiseEvent(EnemyBase enemy)
    {
        OnEnemyKilled?.Invoke(enemy);
    }
}