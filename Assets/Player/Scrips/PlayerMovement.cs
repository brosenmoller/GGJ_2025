using UnityEngine;
using System.Collections;

[RequireComponent(typeof(RaycastController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Horizontal Movement Settings")]
    [SerializeField, Range(0, 1)] private float basicHorizontalDamping = 0.3f;
    [SerializeField, Range(0, 1)] private float horizontalDampingWhenStopping = 0.5f;
    [SerializeField, Range(0, 1)] private float horizontalDampingWhenTurning = 0.4f;

    [Header("Vertical Movement Settings")]
    [SerializeField] private float maxJumpVelocity = 18f;
    [SerializeField, Range(0, 1)] private float jumpCutOff = 0.5f;
    [SerializeField] private float rigidBodyGravityScale = 4f;
    [SerializeField] private float maxFallSpeed = -25;
    [SerializeField] private float fallMultiplier = default;
    [SerializeField] private float bounceForce = default;

    [Header("GroundDetection")]
    [SerializeField] private float jumpDelay = 0.15f;
    [SerializeField] private float groundDelay = 0.15f;

    [Header("JumpSqueeze")]
    [SerializeField] private float xSqueeze = 1.2f;
    [SerializeField] private float ySqueeze = 0.8f;
    [SerializeField] private float squeezeDuration = 0.1f;

    [Header("References")]
    [SerializeField] private Transform spriteHolder;

    public RaycastController raycastController { get; private set; }

    private bool isGrounded;
    private bool wasGrounded;

    private float groundTimer;
    private float jumpTimer;
    private float currentJumpVelocity;

    private float movementX;

    private Rigidbody2D rb;
    private PlayerParticleManager particleManager;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        raycastController = GetComponent<RaycastController>();
        particleManager = GetComponentInChildren<PlayerParticleManager>();
    }

    private void Start() {
        raycastController.UpdateRaycastOrigins();
        raycastController.CalculateRaySpacing();

        isGrounded = GroundCheck();


        rb.gravityScale = rigidBodyGravityScale;
        currentJumpVelocity = maxJumpVelocity;
        SetupControls();
    }

    private void SetupControls() {
        InputManager.Instance.Controls.Gameplay.HorizontalMovement.performed += movement_ctx => UpdateMovementDirection((int)movement_ctx.ReadValue<float>());
        InputManager.Instance.Controls.Gameplay.HorizontalMovement.canceled += _ => UpdateMovementDirection(0);
        InputManager.Instance.Controls.Gameplay.Jump.started += _ => InitiateJump();
        InputManager.Instance.Controls.Gameplay.Jump.canceled += _ => CutJumpVelocity();
    }

    private void Update() {
        wasGrounded = isGrounded;
        isGrounded = GroundCheck();

        // take off (coyote time)
        if (wasGrounded && !isGrounded) {
            groundTimer = Time.time + groundDelay;
            wasGrounded = false;
        }

        // landing
        if (!wasGrounded && isGrounded) {
            StartCoroutine(JumpSqueeze(xSqueeze, ySqueeze, squeezeDuration));

            if (groundTimer < Time.time - groundDelay) {
                //Posible Land animation
                particleManager.GROUNDHIT.Play();
            }
        }
        
    }

    private void InitiateJump() {
        jumpTimer = Time.time + jumpDelay;
    }

    private void CutJumpVelocity() {
        if (jumpTimer > 0) // Release Jumpbutton before touching the ground
        {
            currentJumpVelocity = maxJumpVelocity * (jumpCutOff + 0.1f);
        } else if (rb.linearVelocity.y > 0) {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutOff);
        }
    }

    public void UpdateMovementDirection(int newMovementX) {
        FlipSprite(newMovementX);
        movementX = newMovementX;

        if(isGrounded && !SameDirection(movementX, rb.linearVelocityX))
        {
            if (movementX == 1)
            {
                particleManager.DIRECTIONCHANGELEFT.Play();
            }
            else if (movementX == -1)
            {
                particleManager.DIRECTIONCHANGERIGHT.Play();
            }
        }
    }

    private void FlipSprite(float movementX) {
        if (movementX == 1) {
            spriteHolder.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        } else if (movementX == -1) {
            spriteHolder.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
    }

    private void FixedUpdate() {

        HorizontalMovement();
        if (jumpTimer > Time.time && (groundTimer > Time.time || isGrounded)) {
            Jump(currentJumpVelocity);
        }
        if (rb.linearVelocityY < 0)
        {
            rb.AddForce(Vector3.up * Physics.gravity.y * fallMultiplier * Time.fixedDeltaTime,ForceMode2D.Impulse);
        }
        StepBump();
        JumpBumb();
        SpeedClamps();
    }

    private void HorizontalMovement() {
        float horizontalVelocity = rb.linearVelocity.x;
        horizontalVelocity += movementX;

        if (Mathf.Abs(movementX) < 0.01f) {
            horizontalVelocity *= Mathf.Pow(1f - horizontalDampingWhenStopping, Time.deltaTime * 10f);
        } else if (Mathf.Sign(movementX) != Mathf.Sign(horizontalVelocity)) {
            horizontalVelocity *= Mathf.Pow(1f - horizontalDampingWhenTurning, Time.deltaTime * 10f);
        } else {
            horizontalVelocity *= Mathf.Pow(1f - basicHorizontalDamping, Time.deltaTime * 10f);
        }

        rb.linearVelocity = new Vector2(horizontalVelocity, rb.linearVelocity.y);
    }
    private void Jump(float jumpVelocity) {
        particleManager.JUMP.Play();
        JumpBumpLeft();
        JumpBumpRight();
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpVelocity);

        jumpTimer = 0;
        groundTimer = 0;
        currentJumpVelocity = maxJumpVelocity;
        StartCoroutine(JumpSqueeze(ySqueeze, xSqueeze, squeezeDuration));
    }

    public bool GroundCheck() {

        raycastController.UpdateRaycastOrigins();
        float rayLength = RaycastController.skinWidth * 3f;
        for (int i = 0; i < raycastController.verticalRayCount; i++)
        {
            Vector2 rayOrigin = raycastController.raycastOrigins.bottomLeft;
            rayOrigin += Vector2.right * (raycastController.verticalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, rayLength, raycastController.collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.down * rayLength, Color.red);
            if (hit && hit.collider.gameObject != gameObject)
            {
                return true;
            }
        }

        return false;
    }

    IEnumerator JumpSqueeze(float xSqueeze, float ySqueeze, float seconds) {
        Vector3 originalSize = Vector3.one;
        Vector3 newSize = new Vector3(xSqueeze, ySqueeze, originalSize.z);

        Vector3 oldPos = Vector3.zero;
        Vector3 newPos = new Vector3(0, -.1f, oldPos.z);

        float time = 0f;
        while (time <= 1.0) {
            time += Time.deltaTime / seconds;
            spriteHolder.localScale = Vector3.Lerp(originalSize, newSize, time);
            spriteHolder.localPosition = Vector3.Lerp(oldPos, newPos, time);
            yield return null;
        }
        time = 0f;
        while (time <= 1.0) {
            time += Time.deltaTime / seconds;
            spriteHolder.localScale = Vector3.Lerp(newSize, originalSize, time);
            spriteHolder.localPosition = Vector3.Lerp(newPos, oldPos, time);
            yield return null;
        }
    }


    private void StepBump()
    {
        if(movementX < 0)
            StepBumpLeft();
        if(movementX > 0)
            StepBumpRight();
    }

    private void StepBumpLeft()
    {
        float rayLength = RaycastController.skinWidth * 5f;
        Vector2 rayOrigin = raycastController.raycastOrigins.bottomLeft;

        Debug.DrawRay(rayOrigin, Vector2.left * rayLength, Color.red);
        if (!Physics2D.Raycast(rayOrigin, Vector2.left, rayLength, raycastController.collisionMask))
        {
            return;
        }
        rayOrigin += Vector2.up * 0.03f;
        Debug.DrawRay(rayOrigin, Vector2.left * rayLength, Color.red);
        if (Physics2D.Raycast(rayOrigin, Vector2.left, rayLength, raycastController.collisionMask))
        {
            return;
        }

        rb.position += Vector2.up * rayLength;
        rb.position += Vector2.left * rayLength;
    }

    private void StepBumpRight()
    {
        float rayLength = RaycastController.skinWidth * 5f;
        Vector2 rayOrigin = raycastController.raycastOrigins.bottomRight;

        Debug.DrawRay(rayOrigin, Vector2.right * rayLength, Color.red);
        if (!Physics2D.Raycast(rayOrigin, Vector2.right, rayLength, raycastController.collisionMask))
        {
            return;
        }
        rayOrigin += Vector2.up * 0.03f;
        Debug.DrawRay(rayOrigin, Vector2.right * rayLength, Color.red);
        if (Physics2D.Raycast(rayOrigin, Vector2.right, rayLength, raycastController.collisionMask))
        {
            return;
        }

        rb.position += Vector2.up * rayLength;
        rb.position += Vector2.right * rayLength;
    }


    private void JumpBumb()
    {
        if (rb.linearVelocityY > 0)
        {
            JumpBumpLeft();
            JumpBumpRight();
        }
    }

    private void JumpBumpLeft()
    {
        float rayLength = RaycastController.skinWidth * 10f;
        Vector2 rayOrigin = raycastController.raycastOrigins.topLeft + Vector2.left * RaycastController.skinWidth;

        Debug.DrawRay(rayOrigin, Vector2.up * rayLength, Color.red);
        if (!Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, raycastController.collisionMask))
        {
            return;
        }

        for (int i = 2; i < 7; i++)
        {
            rayOrigin += Vector2.right * 0.05f;
            Debug.DrawRay(rayOrigin, Vector2.up * rayLength, Color.red);
            if (!Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, raycastController.collisionMask))
            {
                rb.position += Vector2.right * 0.05f * i;
                return;
            }
        }
    }

    private void JumpBumpRight()
    {
        float rayLength = RaycastController.skinWidth * 10f;
        Vector2 rayOrigin = raycastController.raycastOrigins.topRight + Vector2.right * RaycastController.skinWidth;

        Debug.DrawRay(rayOrigin, Vector2.up * rayLength, Color.red);
        if (!Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, raycastController.collisionMask))
        {
            return;
        }

        for(int i = 2; i < 7; i++)
        {
            rayOrigin += Vector2.left * 0.05f;
            Debug.DrawRay(rayOrigin, Vector2.up * rayLength, Color.red);
            if (!Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, raycastController.collisionMask))
            {
                rb.position += Vector2.left * 0.05f * i;
                return;
            }
        }
    }

    public void Bounce(Vector2 origin)
    {

        Vector2 direction = (transform.position2D() - origin).normalized;
        if (Vector2.Dot(Vector2.up, direction) > 0)
        {
            rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(Vector2.down * bounceForce, ForceMode2D.Impulse);
        }
       
    }

    public void SpeedClamps()
    {
        Vector2 v = rb.linearVelocity;

        v.y = Mathf.Clamp(v.y, maxFallSpeed, 18);
        rb.linearVelocity = v;
    }

    private bool SameDirection(float a, float b)
    {
        return a * b >= 0.0f;
    }
}
