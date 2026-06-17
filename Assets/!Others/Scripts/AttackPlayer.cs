using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class AttackPlayer : NetworkBehaviour
{
    public Animator animator;
    public NetworkAnimator networkAnimator;
    public KeyCode attackKey = KeyCode.E;
    public float attackDamage = 20f;
    public float attackDuration = 0.35f;
    public float attackCooldown = 0.6f;

    private bool isAttacking;
    private float nextAttackTime;
    private double nextServerAttackTime;
    private readonly HashSet<uint> damagedTargets = new HashSet<uint>();

    private void Start()
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (!isLocalPlayer) return;
        if (!Input.GetKeyDown(attackKey)) return;
        if (Time.time < nextAttackTime) return;

        nextAttackTime = Time.time + attackCooldown;
        PlayAttackAnimation();
        CmdStartAttack();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        TryDamageTarget(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        TryDamageTarget(other);
    }

    private void PlayAttackAnimation()
    {
        if (animator != null)
            animator.SetTrigger("attack");
    }

    [Command]
    private void CmdStartAttack()
    {
        if (NetworkTime.time < nextServerAttackTime)
            return;

        nextServerAttackTime = NetworkTime.time + attackCooldown;
        RpcPlayAttackAnimation();
        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        isAttacking = true;
        damagedTargets.Clear();

        yield return new WaitForSeconds(attackDuration);

        isAttacking = false;
    }

    [ServerCallback]
    private void TryDamageTarget(Collider2D other)
    {
        if (!isAttacking || other == null)
            return;

        PlayerHealth targetHealth = other.GetComponentInParent<PlayerHealth>();

        if (targetHealth == null || targetHealth.netIdentity == netIdentity)
            return;

        uint targetNetId = targetHealth.netIdentity.netId;

        if (damagedTargets.Contains(targetNetId))
            return;

        damagedTargets.Add(targetNetId);
        targetHealth.ServerTakeDamage(attackDamage);
    }

    [ClientRpc]
    private void RpcPlayAttackAnimation()
    {
        if (isLocalPlayer)
            return;

        PlayAttackAnimation();
    }
    //------------------------------------------
}
