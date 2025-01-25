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

    public event Action<BubbleController> OnDestroyed;

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
        UnFreeze();
    }

    public void TriggerFreezeAction() 
    {
        if (!freezeable) return;
        if (isFrozen)
        {
            Pop();
        } 
        else
        {
            Freeze();
        }
    }

    private void Freeze()
    {
        ParticleManager.Instance.PlayParticleAt("Freeze", transform.position);
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
                ParticleManager.Instance.PlayParticleAt("FreezeEnd", transform.position);
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
                Pop();
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
        Pop();
    }

    public void Pop()
    {
        ParticleManager.Instance.PlayParticleAt("BubbleBurst", transform.position);
        OnDestroyed?.Invoke(this);
    }
}