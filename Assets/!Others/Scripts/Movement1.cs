 using Mirror;
using UnityEngine;

public class Movement1 : NetworkBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!isLocalPlayer) return;

        float h = Input.GetAxis("Horizontal");

        // Sprite'ı hareket yönüne göre çevir
        if (h < 0)
            spriteRenderer.flipX = true;  // Sola bak
        else if (h > 0)
            spriteRenderer.flipX = false; // Sağa bak

        if (rb != null)
        {
            rb.linearVelocity = new Vector2(h * speed, rb.linearVelocity.y);
            return;
        }

        Vector3 movement = new Vector3(h, 0f, 0f) * speed * Time.deltaTime;
        transform.Translate(movement);
    }
}
