using Mirror;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Zipziplayarak : NetworkBehaviour
{
    public float jumpForce = 7f;
    public Collider2D groundCheck;
    public LayerMask groundLayers = ~0;
    public float groundTopTolerance = 0.15f;

    private Rigidbody2D rb;
    private Collider2D bodyCollider;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bodyCollider = GetComponent<Collider2D>();

        if (groundCheck == null)
        {
            Transform groundCheckTransform = transform.Find("yerbakan2");
            if (groundCheckTransform != null)
            {
                groundCheck = groundCheckTransform.GetComponent<Collider2D>();
            }
        }
    }

    void Update()
    {
        if (!isLocalPlayer) return;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
            Debug.Log("Jumped!");
        }
    }

    void FixedUpdate()
    {
        isGrounded = CheckGrounded();
    }

    bool CheckGrounded()
    {
        if (groundCheck == null) return false;

        Bounds checkBounds = groundCheck.bounds;
        Collider2D[] groundHits = Physics2D.OverlapBoxAll(
            checkBounds.center,
            checkBounds.size,
            0f,
            groundLayers);

        for (int i = 0; i < groundHits.Length; i++)
        {
            Collider2D hit = groundHits[i];
            if (hit == null || hit == groundCheck || hit == bodyCollider) continue;
            if (hit.transform.IsChildOf(transform)) continue;

            if (hit.bounds.max.y <= checkBounds.center.y + groundTopTolerance)
            {
                return true;
            }
        }

        return false;
    }
}
