using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerCollider : MonoBehaviour
{
    PlayerMovement playerMovement;
    new Rigidbody2D rigidbody2D;

    [SerializeField]
    private ParticleSystem deathParticle;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DeathTrigger"))
        {
            playerMovement.enabled = false;
            rigidbody2D.linearVelocity = Vector2.zero;
            rigidbody2D.gravityScale = 0;
            deathParticle.Play();
            StartCoroutine(DelayedReload());
        }
    }

    private IEnumerator DelayedReload()
    {
        yield return new WaitForSeconds(0.76f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
}