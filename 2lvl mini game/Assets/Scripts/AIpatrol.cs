using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIpatrol : MonoBehaviour
{
    //[HideInInspector]
    public bool isPatroling;
    private bool mustTurn;

    public Rigidbody2D rb;
    public float walkSpeed;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public GameObject player;
    public float firingRange;
    public float folowingRange;
    public int EnemyHitDamage = 20;
    public int timeBTWAttacks = 2;


    public float attackRate = 2f;
    float nextAttackTime = 1f;

    public Transform attackPoint;
    public float attackRange;
    public LayerMask playerLayer;


    public int enemyLife;


    private float distToPlayer;

    Animator myAnimator;

    // Start is called before the first frame update
    void Start()
    {
        isPatroling = true;
        myAnimator = GetComponent<Animator>();
        player = GameObject.Find("Player");
        attackPoint = gameObject.transform.Find("AttackPoint");

        enemyLife = 110;

    }

    // Update is called once per frame
    void Update()
    {
        if(isPatroling)
        {
            Patrol();
        }

        distToPlayer = Vector2.Distance(transform.position, player.transform.position);

        //if player in range then flip him to the rught direction and  attack him 
        if ( distToPlayer <= firingRange )
        {
            isPatroling = false;

            if(Time.time > nextAttackTime)
            {
                if(player.transform.position.x > transform.position.x && transform.localScale.x < 0 || player.transform.position.x < transform.position.x && transform.localScale.x > 0)
                {
                    Flip();
                }

                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
            
            
        }
        else
        {
            myAnimator.SetBool("isAttacking", false);
            isPatroling = true;
        }
    }

    private void FixedUpdate()
    {
        if (isPatroling)
        {
            mustTurn = !Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
        }
    }

    private void Patrol()
    {
        
        if(mustTurn)
        {
            Flip();
        }
        rb.velocity = new Vector2(walkSpeed * Time.fixedDeltaTime, rb.velocity.y);

        myAnimator.SetBool("isWalking", true);
    }

    private void Flip()
    {
        isPatroling = false;
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        walkSpeed *= -1;
        isPatroling = true;
    }


    private void Attack()
    {
        //attack animation 
        myAnimator.SetBool("isAttacking", true);

        //detect enemies in range of attack
        Collider2D[] playerCollider = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);

        
        //low the player life 
        foreach(Collider2D player in playerCollider)
        {
            player.GetComponent<Player>().PlayerHit(EnemyHitDamage);
        }
        


    }


    //draw gizmo on attack point and range
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;


        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public void EnemyTakeDamage(int damage)
    {
        isPatroling = false;

        //play death animation
        myAnimator.SetTrigger("isHurt");

        //lower life 
        enemyLife -= damage;

        //if have no life do Die();
        if (enemyLife <= 0)
        {
            EnemyDeath();
        }

        
    }

    void EnemyDeath()
    {
        myAnimator.SetTrigger("isDead");
        Destroy(gameObject, 0.5f);
    }
}
