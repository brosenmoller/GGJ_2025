using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollider : MonoBehaviour
{
    private PlayerSpawnPoint spawnPoint;

    [SerializeField]
    private ParticleSystem deathParticle;

    [SerializeField]
    private ParticleSystem burstParticle;

    [SerializeField]
    private AnimationCurve respawnCurve;

    [SerializeField]
    private float respawnDurration;

    [SerializeField]
    private Transform destinationTransform;

    public GameObject spawnbubble;

    private void Awake()
    {
        spawnPoint = FindFirstObjectByType<PlayerSpawnPoint>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DeathTrigger"))
        {
            BubbleController[] bubbles = FindObjectsByType<BubbleController>(FindObjectsSortMode.None);
            foreach(BubbleController b in bubbles)
            {
                b.Pop();
            }

            deathParticle.Play();
            spawnPoint.BubbleAndMovePlayer(spawnPoint.transform.position2D(), respawnCurve);
        } 
        else if (collision.CompareTag("LevelEnd"))
        {
            spawnPoint.BubbleAndMovePlayer(destinationTransform.position2D(), respawnCurve, HandleLevelEnd);
        }
    }

    private void HandleLevelEnd()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}