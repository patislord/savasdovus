using Mirror;
using UnityEngine;

public class firescript : NetworkBehaviour
{
        public Animator animator;

    private Rigidbody2D rb;
    public Transform firepoint;
    public GameObject bulletPrefab;
    public KeyCode fireKey = KeyCode.E;
    public float fireCooldown = 0.2f;
        public NetworkAnimator networkAnimator;

    private float nextFireTime;
    private double nextServerFireTime;

    private void Update()
    {
        if (!isLocalPlayer) return;
        if (!Input.GetKey(fireKey)) return;
        if (Time.time < nextFireTime) return;

        nextFireTime = Time.time + fireCooldown;
        CmdShoot();
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
        animator.SetTrigger("attack");
    }
}
