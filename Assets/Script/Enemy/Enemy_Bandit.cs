using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Enemy_Bandit : MonoBehaviour
{
    /// <summary>
    /// 1. Spawn ketika raid
    /// 2. mengejar player
    /// </summary>

    Transform target;
    Enemy_Health eh;
    Rigidbody2D rb;

    [SerializeField] bool drawSpawnRadius;

    [SerializeField] float moveSpd;
    [SerializeField] float aggroDistance;
    Vector2 runDir;

    [SerializeField] GameObject attackHitBox;
    [SerializeField] float attackDur = .5f;
    [SerializeField] float attackDelay = 2f;
    float attackTimer, attackingTimer;

    [SerializeField] float waitTime = 2f;
    float waitTimer;


    private void Awake()
    {
        eh = GetComponent<Enemy_Health>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        target = eh.player;
    }

    // Update is called once per frame
    void Update()
    {
        waitTimer += Time.deltaTime;
        if (waitTimer > waitTime)
        {
            if (!IsNearPlayer())
                rb.position = Vector2.MoveTowards(rb.position, target.position, moveSpd * Time.deltaTime);
            else
            {
                //Try to attack player
                attackTimer += Time.deltaTime;
                if (attackTimer > attackDelay)
                {
                    attackHitBox.SetActive(true);
                    attackingTimer += Time.deltaTime;
                    if (attackingTimer > attackDur)
                    {
                        attackHitBox.SetActive(false);
                        attackingTimer = 0;
                        attackTimer = 0;
                        waitTimer = 0;
                    }
                }
            }
        }
    }

    bool IsNearPlayer()
    {
        return Vector2.Distance(transform.position, target.position) < aggroDistance;
    }

    #region DEBUG
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (drawSpawnRadius)
            Gizmos.DrawWireSphere(transform.position, aggroDistance);
    }
#endif
    #endregion
}
