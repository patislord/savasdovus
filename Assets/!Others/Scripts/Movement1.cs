using Mirror;
using UnityEngine;

public class Movement1 : NetworkBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!isLocalPlayer) return;

        float h = Input.GetAxis("Horizontal");

        if (rb != null)
        {
            rb.linearVelocity = new Vector2(h * speed, rb.linearVelocity.y);
            return;
        }

        Vector3 movement = new Vector3(h, 0f, 0f) * speed * Time.deltaTime;
        transform.Translate(movement);
    }
}
