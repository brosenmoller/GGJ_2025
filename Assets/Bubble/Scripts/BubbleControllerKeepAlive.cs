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

    private void KillPlayer()
    {
        ParticleManager.Instance.PlayParticleAt("DeathWave", transform.position);
        spawnPoint.Respawn();
    }
}
