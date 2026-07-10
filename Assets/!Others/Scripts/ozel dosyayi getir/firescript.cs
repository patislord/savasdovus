using Mirror;
using UnityEngine;

namespace FireScript
{
    public class firescript : NetworkBehaviour
{
    public Animator animator;
    public Transform firepoint;
    public GameObject bulletPrefab;
    public KeyCode fireKey = KeyCode.R;
    public float fireCooldown = 0.2f;

    private float nextFireTime;
    private double nextServerFireTime;

    private void Start()
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (!isLocalPlayer) return;
        if (!Input.GetKey(fireKey)) return;
        if (Time.time < nextFireTime) return;

        nextFireTime = Time.time + fireCooldown;
        PlayAttackAnimation();
        CmdShoot();
    }

    private void PlayAttackAnimation()
    {
        if (animator != null)
            animator.SetTrigger("attack");
    }

    [Command]
    private void CmdShoot()
    {
        if (NetworkTime.time < nextServerFireTime)
            return;

        if (firepoint == null || bulletPrefab == null)
            return;

        nextServerFireTime = NetworkTime.time + fireCooldown;

        GameObject bullet = Instantiate(bulletPrefab, firepoint.position, firepoint.rotation);
        NetworkServer.Spawn(bullet);
        RpcPlayAttackAnimation();
    }

    [ClientRpc]
    private void RpcPlayAttackAnimation()
    {
        if (isLocalPlayer)
            return;

        PlayAttackAnimation();
    }
}
}
