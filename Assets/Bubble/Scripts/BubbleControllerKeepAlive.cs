using UnityEngine;

public class BubbleControllerKeepAlive : BubbleController
{
    private PlayerSpawnPoint spawnPoint;
    private void Awake()
    {
        spawnPoint = FindFirstObjectByType<PlayerSpawnPoint>();
    }

    private void OnEnable()
    {
        OnDestroyed += spawnPoint.Respawn;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        OnDestroyed -= spawnPoint.Respawn;
    }
}
