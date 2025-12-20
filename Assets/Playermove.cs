using UnityEngine;
using UnityEngine.InputSystem;

public class JRPGController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private bool canMove = true;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 lastMoveDirection = Vector2.down;

    // Animation parameter names
    private const string ANIM_MOVE_X = "MoveX";
    private const string ANIM_MOVE_Y = "MoveY";
    private const string ANIM_IS_MOVING = "IsMoving";

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (animator == null)
            animator = GetComponent<Animator>();

        // Check if Rigidbody2D exists
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D not found! Please add a Rigidbody2D component.");
        }
        else
        {
            Debug.Log("Player controller initialized successfully!");
        }
    }

    void Update()
    {
        if (canMove)
        {
            // Direct keyboard input - works without Input System setup
            moveInput = Vector2.zero;

            if (Keyboard.current != null)
            {
                // New Input System (Unity 6)
                if (Keyboard.current.wKey.isPressed) moveInput.y += 1;
                if (Keyboard.current.sKey.isPressed) moveInput.y -= 1;
                if (Keyboard.current.aKey.isPressed) moveInput.x -= 1;
                if (Keyboard.current.dKey.isPressed) moveInput.x += 1;
            }

            // Normalize diagonal movement
            if (moveInput.magnitude > 1)
                moveInput.Normalize();

            // Update last direction if moving
            if (moveInput != Vector2.zero)
            {
                lastMoveDirection = moveInput;
            }

            // Debug - remove this once it works
            if (moveInput != Vector2.zero)
            {
                Debug.Log($"Moving: {moveInput}");
            }
        }
        else
        {
            moveInput = Vector2.zero;
        }

        UpdateAnimation();
    }

    void FixedUpdate()
    {
        if (rb != null)
        {
            rb.linearVelocity = moveInput * moveSpeed;
        }
    }

    void UpdateAnimation()
    {
        if (animator == null) return;

        bool isMoving = moveInput.magnitude > 0;

        animator.SetFloat(ANIM_MOVE_X, lastMoveDirection.x);
        animator.SetFloat(ANIM_MOVE_Y, lastMoveDirection.y);
        animator.SetBool(ANIM_IS_MOVING, isMoving);
    }

    public void SetCanMove(bool value)
    {
        canMove = value;
        if (!canMove && rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    public void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }

    public Vector2 GetFacingDirection()
    {
        return lastMoveDirection;
    }
}