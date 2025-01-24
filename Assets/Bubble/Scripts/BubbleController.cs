using System;
using UnityEngine;

public class BubbleController : MonoBehaviour, IFreezable
{
    [SerializeField] private float freezeTime;
    [SerializeField] private Color frozenColor;
    [SerializeField] private Color normalColor;
    [SerializeField] private SpriteRenderer spriteHolder;

    public event Action OnDestroyed;

    private float speed;
    private Vector2 direction;
    private Rigidbody2D rigidBody2D;
    private bool isFrozen;
    private Collider2D bubbleCollider;

    private float freezeEndTime;

    public void Setup(float speed, Vector2 direction) 
    {
        this.speed = speed;
        this.direction = direction;
        spriteHolder.color = normalColor;

        rigidBody2D = GetComponent<Rigidbody2D>();
        bubbleCollider = GetComponent<Collider2D>();
        bubbleCollider.isTrigger = true;
    }

    public void Freeze() 
    {
        if (isFrozen)
        {
            OnDestroyed?.Invoke();
        } 
        else
        {
            spriteHolder.color = frozenColor;
            freezeEndTime = Time.time + freezeTime;
            rigidBody2D.linearVelocity = Vector2.zero;
            isFrozen = true;
            bubbleCollider.isTrigger = false;
        }
    }

    private void Update()
    {
        if (isFrozen) {
            if (Time.time > freezeEndTime) {
                spriteHolder.color = normalColor;
                isFrozen = false;
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