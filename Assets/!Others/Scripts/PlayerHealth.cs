using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class PlayerHealth : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnHpchange))]public float hp = 100;
    public UnityEngine.UI.Image healthBar;
    private bool isDead;

    private void Update()
    {
        if (!isLocalPlayer) return;
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(10);
            Debug.Log("oldu");
        }
    }

    public void TakeDamage(float damage)
    {
        if (isLocalPlayer)
        {
            CMDTakeDamage(damage);
        }
    }


    [Command]
    public void CMDTakeDamage(float damage)
    {
        if(isDead || damage < 0) return;

        hp -= damage;

        if (hp <= 0)
        {
            Die();
        }
    }

    [Server]
    private void Die()
    {
        isDead = true;
        hp = 0;

        NetworkConnectionToClient ownerConnection = connectionToClient;
        if (ownerConnection == null || NetworkManager.singleton == null || NetworkManager.singleton.playerPrefab == null)
        {
            NetworkServer.Destroy(gameObject);
            return;
        }

        GameObject fakePlayer = Instantiate(
            NetworkManager.singleton.playerPrefab,
            transform.position,
            transform.rotation
        );

        NetworkServer.ReplacePlayerForConnection(ownerConnection, fakePlayer, ReplacePlayerOptions.KeepActive);
        Destroy(gameObject, 0.1f);
    }

    private void OnHpchange(float oldHp, float newHp)
    {
        healthBar.fillAmount = newHp / 100;
    }
}
