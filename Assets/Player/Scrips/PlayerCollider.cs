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
    public GameObject spawnbubble;


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
            BubbleController[] bubbles = FindObjectsByType<BubbleController>(FindObjectsSortMode.None);
            foreach(BubbleController b in bubbles)
            {
                b.pop();
            }
            playerMovement.enabled = false;
            playerMovement.raycastController.collider.enabled = false;
            rigidbody2D.linearVelocity = Vector2.zero;
            rigidbody2D.simulated = false;
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
        while (transform.position2D() != spawnPoint.transform.position2D())
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
        rigidbody2D.simulated = true;
        playerMovement.enabled = true;
        playerMovement.raycastController.collider.enabled = true;
    }
    
}