using UnityEngine;
using System.Collections;
using Unity.Cinemachine;
public class PlayerSpawnPoint : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve spawnCurve;
    [SerializeField]
    private float spawnDurration;
    [SerializeField]
    private ParticleSystem burstParticle;
    private IEnumerator Start()
    {
        PlayerMovement player = FindAnyObjectByType<PlayerMovement>();
        Rigidbody2D rigidbody2D = player.GetComponent<Rigidbody2D>();
        GameObject spawnbubble = player.GetComponent<PlayerCollider>().spawnbubble;
        CinemachineCamera cinemachineCamera = FindFirstObjectByType<CinemachineCamera>();
        player.enabled = false;
        player.raycastController.collider.enabled = false;
        rigidbody2D.linearVelocity = Vector2.zero;
        rigidbody2D.simulated = false;
        spawnbubble.SetActive(true);
        Vector2 startPostion = rigidbody2D.position;
        float time = 0;
        while (player.transform.position != transform.position)
        {
            time += Time.deltaTime;
            player.transform.position = Vector2.Lerp(startPostion, transform.position2D(), spawnCurve.Evaluate(time / spawnDurration));
            if (time >= spawnDurration)
            {
                player.transform.position = transform.position2D();
            }
            yield return null;
        }
        spawnbubble.SetActive(false);
        Instantiate(burstParticle, transform.position, Quaternion.identity);
        rigidbody2D.simulated = true;
        player.enabled = true;
        player.raycastController.collider.enabled = true;
        cinemachineCamera.Target.TrackingTarget = player.transform;
    }



    


}
