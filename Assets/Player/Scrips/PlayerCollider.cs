using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerCollider : MonoBehaviour
{
    PlayerMovement playerMovement;
    new Rigidbody2D rigidbody2D;
    PlayerSpawnPoint spawnPoint;
    [SerializeField]
    private ParticleSystem deathParticle;
    [SerializeField]
    private ParticleSystem burstParticle;
    [SerializeField]
    private AnimationCurve respawnCurve;
    [SerializeField]
    private float respawnDurration;
    [SerializeField]
    private GameObject spawnbubble;
    private float oldGravityScale;


    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        spawnPoint = FindFirstObjectByType<PlayerSpawnPoint>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DeathTrigger"))
        {
            playerMovement.enabled = false;
            playerMovement.raycastController.collider.enabled = false;
            rigidbody2D.linearVelocity = Vector2.zero;
            oldGravityScale = rigidbody2D.gravityScale;
            rigidbody2D.gravityScale = 0;
            deathParticle.Play();
            StartCoroutine(Respawn());
        }
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(0.7f);
        spawnbubble.SetActive(true);
        Vector2 startPostion = rigidbody2D.position;
        float time = 0;
        while (rigidbody2D.position != spawnPoint.transform.position2D())
        {
            time += Time.deltaTime;
            transform.position = Vector2.Lerp(startPostion, spawnPoint.transform.position2D(), respawnCurve.Evaluate(time/respawnDurration));
            if(time >= respawnDurration)
            {
                transform.position = spawnPoint.transform.position2D();
            }
            yield return null;
        }
        spawnbubble.SetActive(false);
        Instantiate(burstParticle, transform.position, Quaternion.identity);
        rigidbody2D.gravityScale = oldGravityScale;
        playerMovement.enabled = true;
        playerMovement.raycastController.collider.enabled = true;
    }
    
}