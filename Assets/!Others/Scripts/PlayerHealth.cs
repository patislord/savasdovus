using UnityEngine;
using UnityEngine.UI;
using Mirror;
using UnityEditor;
using Microsoft.Unity.VisualStudio.Editor;
public class PlayerHealth : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnHpchange))]public float hp = 100;
    public UnityEngine.UI.Image healthBar;

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
        if(damage < 0) return;
        hp -= damage;
    }
    private void OnHpchange(float oldHp, float newHp)
    {
        healthBar.fillAmount = newHp / 100;
    }
}
