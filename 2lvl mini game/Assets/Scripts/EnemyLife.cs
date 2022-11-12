using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLife : MonoBehaviour
{

    public int enemyLife;

    Animator myAnimator;

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        enemyLife = 110;
    }

    public void EnemyTakeDamage(int damage)
    {
        
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
