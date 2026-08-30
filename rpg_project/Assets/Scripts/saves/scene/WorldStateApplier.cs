using UnityEngine;

public class WorldStateApplier : IWorldStateApplier
{
    public void Apply(WorldSnapshot snapshot)
    {
        if (snapshot == null) return;

        ApplyPlayerSnapshot(snapshot.player);
        ApplyEnemiesSnapshot(snapshot);
    }

    private void ApplyPlayerSnapshot(PlayerSnapshot playerSnapshot)
    {
        if (playerSnapshot == null) return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        var characterController = player.GetComponent<CharacterController>();
        if (characterController != null)
        {
            characterController.enabled = false;
        }

        player.transform.position = playerSnapshot.position;

        if (characterController != null)
        {
            characterController.enabled = true;
        }

        var health = player.GetComponent<HealthComponent>();
        if (health != null)
        {
            health.LoadHealth(playerSnapshot.health);
        }
    }

    private void ApplyEnemiesSnapshot(WorldSnapshot snapshot)
    {
        var spawner = Object.FindAnyObjectByType<UniversalSpawner>();
        if (spawner == null) return;

        spawner.SetDeathCount(snapshot.deathCount);
        spawner.RestoreFromSave(snapshot.enemies);
    }
}
