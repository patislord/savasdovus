using Mirror;
using UnityEngine;

public class bordermurderer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth playerHealth = other.GetComponentInParent<PlayerHealth>();
        if (playerHealth == null) return;

        if (NetworkServer.active)
        {
            playerHealth.Kill();
            return;
        }

        if (playerHealth.isLocalPlayer)
        {
            playerHealth.TakeDamage(float.MaxValue);
        }
    }
}
