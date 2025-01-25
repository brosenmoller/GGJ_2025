public class BubbleControllerKeepAlive : BubbleController
{
    private PlayerSpawnPoint spawnPoint;
    private void Awake()
    {
        spawnPoint = FindFirstObjectByType<PlayerSpawnPoint>();
    }

    private void OnEnable()
    {
        OnDestroyed += KillPlayer;
    }

    protected void OnDisable()
    {
        OnDestroyed -= KillPlayer;
    }

    private void KillPlayer()
    {
        spawnPoint.Respawn();
    }
}
