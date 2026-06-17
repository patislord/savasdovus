using Mirror;
using UnityEngine;

public class Movement1 : NetworkBehaviour
{
    public float speed = 5f;
    public Animator animator;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    [SyncVar(hook = nameof(OnFacingLeftChanged))]
    private bool facingLeft;

    private static readonly int IsWalk = Animator.StringToHash("IsWalk");

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        ApplyFacingDirection(facingLeft);
    }

    private void Update()
    {
        if (!isLocalPlayer) return;

        float h = Input.GetAxis("Horizontal");
        bool isWalking = Mathf.Abs(h) > 0.01f;

        if (animator != null)
            animator.SetBool(IsWalk, isWalking);

        if (h < 0)
            SetFacingDirection(true);
        else if (h > 0)
            SetFacingDirection(false);

        if (rb != null)
        {
            rb.linearVelocity = new Vector2(h * speed, rb.linearVelocity.y);
            return;
        }

        Vector3 movement = new Vector3(h, 0f, 0f) * speed * Time.deltaTime;
        transform.Translate(movement);
    }

    private void SetFacingDirection(bool newFacingLeft)
    {
        if (facingLeft == newFacingLeft)
            return;

        ApplyFacingDirection(newFacingLeft);
        CmdSetFacingDirection(newFacingLeft);
    }

    private void ApplyFacingDirection(bool newFacingLeft)
    {
        facingLeft = newFacingLeft;

        if (spriteRenderer != null)
            spriteRenderer.flipX = newFacingLeft;
    }

    private void OnFacingLeftChanged(bool oldFacingLeft, bool newFacingLeft)
    {
        ApplyFacingDirection(newFacingLeft);
    }

    [Command]
    private void CmdSetFacingDirection(bool newFacingLeft)
    {
        facingLeft = newFacingLeft;
    }
}
