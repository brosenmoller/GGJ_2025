using UnityEngine;
using System.Collections;
using Unity.Cinemachine;
using System;

public class PlayerSpawnPoint : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve spawnCurve;
    [SerializeField]
    private AnimationCurve respawnCurve;
    [SerializeField]
    private float spawnDurration;

    [SerializeField]
    private GameObject spawnBubble;

    public bool Respawning { get; private set; }

    private PlayerMovement player;
    private Rigidbody2D rigidBody2D;
    private CinemachineCamera cinemachineCamera;
    private PlayerFreezeController freezeController;

    private void Awake()
    {
        player = FindAnyObjectByType<PlayerMovement>();
        freezeController = player.GetComponent<PlayerFreezeController>();
        rigidBody2D = player.GetComponent<Rigidbody2D>();
        cinemachineCamera = FindFirstObjectByType<CinemachineCamera>();
    }

    private IEnumerator Start()
    {
        yield return ParticleManager.WaitUntillExists;
        Respawning = true;
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
        Respawning = true;
        player.enabled = false;
        player.raycastController.collider.enabled = false;
        rigidBody2D.linearVelocity = Vector2.zero;
        rigidBody2D.simulated = false;
        freezeController.enabled = false;
        spawnBubble.SetActive(true);
    }

    private void UnBubblePlayer()
    {
        Respawning = false;
        freezeController.enabled = true;
        spawnBubble.SetActive(false);
        AudioManager.Instance.PlayOneShotRandomPitchFromDictonary("BubbleDeath", transform.position);
        ParticleManager.Instance.PlayParticleAt("BubbleBurst", transform.position);
        rigidBody2D.simulated = true;
        player.enabled = true;
        player.HardReset();
        player.raycastController.collider.enabled = true;
        cinemachineCamera.Target.TrackingTarget = player.transform;
    }

    public void Respawn()
    {
        if (Respawning) { return; }
        Respawning = true;

        PlayerPrefs.SetInt(SaveSystem.DEATHS_SAVE, PlayerPrefs.GetInt(SaveSystem.DEATHS_SAVE) + 1);
        PlayerPrefs.Save();

        AudioManager.Instance.PlayOneShotRandomPitchFromDictonary("Death", transform.position);
        ParticleManager.Instance.PlayParticleAt("Death", player.transform.position);

        BubbleController[] bubbles = FindObjectsByType<BubbleController>(FindObjectsSortMode.None);
        foreach (BubbleController b in bubbles)
        {
            b.Pop();
        }

        StartCoroutine(DelayedBubble());
    }

    IEnumerator DelayedBubble()
    {
        player.enabled = false;
        player.raycastController.collider.enabled = false;
        rigidBody2D.linearVelocity = Vector2.zero;
        rigidBody2D.simulated = false;
        freezeController.enabled = false;
        yield return new WaitForSeconds(0.4f);
        BubbleAndMovePlayer(transform.position2D(), respawnCurve);
    }
}