using UnityEngine;
using System.Collections;
using Unity.Cinemachine;
using System;

public class PlayerSpawnPoint : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve spawnCurve;
    [SerializeField]
    private float spawnDurration;
    [SerializeField]
    private ParticleSystem burstParticle;

    [SerializeField]
    private GameObject spawnBubble;

    private PlayerMovement player;
    private Rigidbody2D rigidBody2D;
    private CinemachineCamera cinemachineCamera;

    private void Start()
    {
        player = FindAnyObjectByType<PlayerMovement>();
        rigidBody2D = player.GetComponent<Rigidbody2D>();
        cinemachineCamera = FindFirstObjectByType<CinemachineCamera>();

        BubbleAndMovePlayer(transform.position2D(), spawnCurve);
    }

    public void BubbleAndMovePlayer(Vector2 destination)
    {
        StartCoroutine(BubbleAndMovePlayerRoutine(destination, spawnCurve, null));
    }

    public void BubbleAndMovePlayer(Vector2 destination, AnimationCurve curve)
    {
        StartCoroutine(BubbleAndMovePlayerRoutine(destination, curve, null));
    }

    public void BubbleAndMovePlayer(Vector2 destination, AnimationCurve curve, Action callback)
    {
        StartCoroutine(BubbleAndMovePlayerRoutine(destination, curve, callback));
    }

    private IEnumerator BubbleAndMovePlayerRoutine(Vector2 destination, AnimationCurve curve, Action callback)
    {
        BubblePlayer();

        Vector2 startPostion = rigidBody2D.position;
        float time = 0;
        while (!player.transform.position2D().Approx(destination))
        {
            time += Time.deltaTime;
            player.transform.position = Vector2.Lerp(startPostion, destination, curve.Evaluate(time / spawnDurration));
            if (time >= spawnDurration)
            {
                player.transform.position = destination;
            }
            yield return null;
        }

        UnBubblePlayer();

        callback?.Invoke();
    }

    private void BubblePlayer()
    {
        player.enabled = false;
        player.raycastController.collider.enabled = false;
        rigidBody2D.linearVelocity = Vector2.zero;
        rigidBody2D.simulated = false;
        spawnBubble.SetActive(true);
    }

    private void UnBubblePlayer()
    {
        spawnBubble.SetActive(false);
        Instantiate(burstParticle, transform.position, Quaternion.identity);
        rigidBody2D.simulated = true;
        player.enabled = true;
        player.raycastController.collider.enabled = true;
        cinemachineCamera.Target.TrackingTarget = player.transform;
    }
}