using UnityEngine;

public class BubbleController : MonoBehaviour, IFreezable
{
    private float speed;
    private Vector2 direction;
    private Rigidbody2D rigidBody2D;
    private bool isFrozen;
    private Collider2D bubbleCollider;

    public void Setup(float speed, Vector2 direction) {
        this.speed = speed;
        this.direction = direction;
        rigidBody2D = GetComponent<Rigidbody2D>();
        bubbleCollider = GetComponent<Collider2D>();
        bubbleCollider.enabled = false;
    }
    
    public void Freeze() {
        isFrozen = true;
        bubbleCollider.enabled = true;
    }

    private void Update() {
        if (isFrozen) { 
            rigidBody2D.linearVelocity = Vector2.zero;
            return; 
        }

        rigidBody2D.linearVelocity = speed * Time.deltaTime * direction;
    }
}