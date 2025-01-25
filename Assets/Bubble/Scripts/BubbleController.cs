using System;
using UnityEngine;
using UnityEngine.Splines;

public class BubbleController : MonoBehaviour
{
    [Serializable]
    public class Config
    {
        [field: SerializeField] public float SplineDuration { get; private set; }
        [field: SerializeField] public SplineContainer Spline { get; private set; }
        [field: SerializeField] public bool IsSplineLoopingEnabled { get; private set; }
        [field: SerializeField] public AnimationCurve SplineCurve { get; private set; }
    }

    [Header("Bubble Settings")]
    [SerializeField] private float freezeTime;
    [SerializeField] private Color frozenColor;
    [SerializeField] private Color normalColor;
    [SerializeField] private SpriteRenderer spriteHolder;

    [Header("Particles")]
    [SerializeField] private ParticleSystem burstParticle;
    [SerializeField] private ParticleSystem FreezeParticle;
    [SerializeField] private ParticleSystem FreezeEndParticle;

    public event Action OnDestroyed;

    private Rigidbody2D rigidBody2D;
    private Collider2D bubbleCollider;
    
    private bool isFrozen;
    [SerializeField]
    private bool freezeable = true;
    [SerializeField]
    private bool isBouncy;

    private float freezeEndTime;

    private float splineTimeValue;
    private bool isDirectionForward;
    private bool spawnedUnfreezeParticle;

    private Config config;

    public void Setup(Config config) 
    {
        this.config = config;

        rigidBody2D = GetComponent<Rigidbody2D>();
        bubbleCollider = GetComponent<Collider2D>();
        isDirectionForward = true;
        OnDestroyed += spawnBurstParticle;
        UnFreeze();
    }

    public void TriggerFreezeAction() 
    {
        if (!freezeable) return;
        if (isFrozen)
        {
            OnDestroyed?.Invoke();
        } 
        else
        {
            Freeze();
        }
    }

    private void Freeze()
    {
        //TODO: Replace with pool instead of instantiate
        Instantiate(FreezeParticle, transform.position, Quaternion.identity);
        isFrozen = true;
        bubbleCollider.isTrigger = false;
        freezeEndTime = Time.time + freezeTime;
        rigidBody2D.linearVelocity = Vector2.zero;
        spriteHolder.color = frozenColor;
        spawnedUnfreezeParticle = false;
    }

    private void UnFreeze()
    {
        isFrozen = false;
        spriteHolder.color = normalColor;
        bubbleCollider.isTrigger = true;
    }

    private void Update()
    {
        if (isFrozen) {
            if(Time.time > freezeEndTime - 1 && !spawnedUnfreezeParticle)
            {
                //TODO: Replace with pool instead of instantiate
                Instantiate(FreezeEndParticle, transform.position, Quaternion.identity);
                spawnedUnfreezeParticle = true;
            }
            if (Time.time > freezeEndTime) {
                UnFreeze();
            }

            return; 
        }

        float moveDifference = Time.deltaTime / config.SplineDuration;
        if (isDirectionForward) { splineTimeValue += moveDifference; }
        else { splineTimeValue -= moveDifference; }

        if (splineTimeValue > 1)
        {
            if (config.IsSplineLoopingEnabled)
            {
                isDirectionForward = !isDirectionForward;
            }
            else
            {
                OnDestroyed?.Invoke();
            }
        } else if (splineTimeValue < 0)
        {
            isDirectionForward = !isDirectionForward;
        }

        float splineDistance = config.SplineCurve.Evaluate(splineTimeValue);
        Vector3 currentPosition = config.Spline.EvaluatePosition(splineDistance);
        rigidBody2D.MovePosition(currentPosition);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isBouncy)
        {
            if( collision.TryGetComponent<PlayerMovement>(out var player))
            {
                player.Bounce(transform.position);
                return;
            }
        }
        OnDestroyed?.Invoke();
    }

    //TODO: Replace with pool instead of instantiate
    private void spawnBurstParticle()
    {
        Instantiate(burstParticle, transform.position, Quaternion.identity);
    }
    private void OnDisable()
    {
        OnDestroyed -= spawnBurstParticle;
    }
}