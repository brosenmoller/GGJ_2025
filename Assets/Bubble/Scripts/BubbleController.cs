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

    private Spline spline;

    private Rigidbody2D rigidBody2D;
    private Collider2D bubbleCollider;
    
    private float speed;
    private Vector2 direction;
    private bool isFrozen;

    private float freezeEndTime;

    public void Setup(float speed, Vector2 direction, Spline spline) 
    {
        this.speed = speed;
        this.direction = direction;
        this.spline = spline;

        rigidBody2D = GetComponent<Rigidbody2D>();
        bubbleCollider = GetComponent<Collider2D>();

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

        rigidBody2D.linearVelocity = speed * Time.deltaTime * direction;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnDestroyed?.Invoke();
    }
}