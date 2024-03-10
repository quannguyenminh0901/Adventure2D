using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol Points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("Enemy")]
    [SerializeField] private Transform enemy;

    [Header("Movement")]
    [SerializeField] private float speed;
    private Vector3 initScale;
    private bool movingLeft;

    [Header("Enemy Behaviour")]
    [SerializeField]private float idleDuration;
    private float idleTimer;

    [Header("Enemy Animator")]
    [SerializeField] private Animator anim;

    private void Awake()
    {
        initScale = transform.localScale;
    }

    private void OnDisable()
    {
        anim.SetBool("moving", false);
    }

    private void Update()
    {
        if (movingLeft)
        {
            if (enemy.position.x >= leftEdge.position.x)
                MoveInDirect(-1);
            else
                DirectChange();
        }
        else
        {
            if (enemy.position.x <= rightEdge.position.x)
                MoveInDirect(1);
            else
                DirectChange();
        }
    }

    private void DirectChange()
    {
        anim.SetBool("moving", false);

        idleTimer += Time.deltaTime;

        if(idleTimer > idleDuration)
            movingLeft = !movingLeft;
    }

    private void MoveInDirect(int _direcrion)
    {
        idleTimer = 0;
        anim.SetBool("moving", true);
        // Mặt của Enemy quay theo hướng di chuyển
        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * _direcrion, initScale.y, initScale.z);

        // Di chuyển theo hướng
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direcrion * speed, enemy.position.y, enemy.position.z);
    }
}

