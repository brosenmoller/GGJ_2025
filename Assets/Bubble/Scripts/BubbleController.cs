using System;
using UnityEngine;
using UnityEngine.Splines;

public class BubbleController : MonoBehaviour
{
    [Header("Bubble Settings")]
    [SerializeField] private float freezeTime;
    [SerializeField] private Color frozenColor;
    [SerializeField] private Color normalColor;
    [SerializeField] private SpriteRenderer spriteHolder;

    public event Action OnDestroyed;

    private Rigidbody2D rigidBody2D;
    private Collider2D bubbleCollider;
    
    private SplineContainer spline;
    private float speed;
    private bool isSplineLoopingEnabled;

    private bool isFrozen;

    private float freezeEndTime;

    private float splineTimeValue;
    private float splineLength;
    private bool isDirectionForward;

    public void Setup(float speed, SplineContainer spline, bool isSplineLoopingEnabled = false) 
    {
        this.speed = speed;
        this.spline = spline;
        this.isSplineLoopingEnabled = isSplineLoopingEnabled;

        rigidBody2D = GetComponent<Rigidbody2D>();
        bubbleCollider = GetComponent<Collider2D>();
        splineLength = spline.CalculateLength();
        isDirectionForward = true;

        UnFreeze();
    }

    public void TriggerFreezeAction() 
    {
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
        isFrozen = true;
        bubbleCollider.isTrigger = false;
        freezeEndTime = Time.time + freezeTime;
        rigidBody2D.linearVelocity = Vector2.zero;
        spriteHolder.color = frozenColor;
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
            if (Time.time > freezeEndTime) {
                UnFreeze();
            }

            return; 
        }

        float moveDifference = speed * Time.deltaTime / splineLength;
        if (isDirectionForward) { splineTimeValue += moveDifference; }
        else { splineTimeValue -= moveDifference; }

        if (splineTimeValue > 1)
        {
            if (isSplineLoopingEnabled)
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

        Vector3 currentPosition = spline.EvaluatePosition(splineTimeValue);
        rigidBody2D.MovePosition(currentPosition);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnDestroyed?.Invoke();
    }
}