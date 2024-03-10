using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [Header("Atk Para")]
    [SerializeField] private float atkCooldown;
    [SerializeField] private float range;
    [SerializeField] private int dame;

    [Header("Collider Para")]
    [SerializeField] private float distance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;
    private float CDTimer = Mathf.Infinity;

    [Header("Atk Sound")]
    [SerializeField] private AudioClip atkSound;

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
                anim.SetTrigger("meleeAtk");
                SoundManager.instance.PlaySound(atkSound);
            }
        }

        if (enemyPatrol != null)
        {
            enemyPatrol.enabled = !PlayerInSight();
        }
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

    private void DamePlayer()
    {
        if (PlayerInSight())
        {
            playerHealth.TakeDamage(dame);
        }
    }
}
