using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    [Header("Atk Para")]
    [SerializeField] private float atkCooldown;
    [SerializeField] private float range;
    [SerializeField] private int dame;

    [Header("Ranged Para")]
    [SerializeField] private Transform firepoint;
    [SerializeField] private GameObject[] fireball;

    [Header("Fireball Sound")]
    [SerializeField] private AudioClip fireballSound;

    [Header("Collider Para")]
    [SerializeField] private float distance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;
    private float CDTimer = Mathf.Infinity;

    private Animator anim;
    private Health playerHealth;

    private EnemyPatrol enemyPatrol;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }
    private void Update()
    {
        CDTimer += Time.deltaTime;

        //Tấn công khi player trong phạm vi
        if (PlayerInSight())
        {
            if (CDTimer >= atkCooldown && playerHealth.currentHealth > 0)
            {
                CDTimer = 0;
                anim.SetTrigger("rangedAtk");
            }
        }

        if (enemyPatrol != null)
        {
            enemyPatrol.enabled = !PlayerInSight();
        }
    }

    private void RangedAtk()
    {
        SoundManager.instance.PlaySound(fireballSound);
        CDTimer = 0;
        fireball[FindFireball()].transform.position = firepoint.position;
        fireball[FindFireball()].GetComponent<EnemyProjectile>().ActivateProjectile();
    }

    private int FindFireball()
    {
        for (int i = 0; i < fireball.Length; i++)
        {
            if (!fireball[i].activeInHierarchy)
                return i;
        }
        return 0;
    }
    private bool PlayerInSight()
    {
        RaycastHit2D hit =
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * distance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
        {
            playerHealth = hit.collider.GetComponent<Health>();
        }

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x * distance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }
}
