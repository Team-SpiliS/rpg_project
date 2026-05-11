using UnityEngine;

public class KillSoundManager : MonoBehaviour
{
    public EnemyKilledEventSO enemyKilledEvent;
    public AudioClip rewardSound;              
    public AudioSource audioSource; 

    private int _killCounter = 0;

    private void OnEnable()
    {
        if (enemyKilledEvent != null)
            enemyKilledEvent.OnEnemyKilled += OnEnemyKilled;
    }

    private void OnDisable()
    {
        if (enemyKilledEvent != null)
            enemyKilledEvent.OnEnemyKilled -= OnEnemyKilled;
    }

    private void OnEnemyKilled(EnemyBase enemy)
    {
        if (enemy is BossEnemy) return;

        _killCounter++;

        if (_killCounter % 5 == 0)
        {
            PlayRewardSound();
        }
    }

    private void PlayRewardSound()
    {
        if (audioSource != null && rewardSound != null)
        {
            audioSource.PlayOneShot(rewardSound);
        }
    }
}