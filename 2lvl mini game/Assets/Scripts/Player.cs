using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;




public class Player : MonoBehaviour
{

    public float moveSpeed = 5f;
    public float jumpForce = 2f;
    public int playerLife;
    public int playerMaxLife = 100;

    public GameObject firePoint;

    public HealthBar healthBar;

    public SceneLoader sceneLoader;
    
    bool facingRight = true;




    Rigidbody2D myRB;
    Animator myAnimator;
    Collider2D myCollider;

    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCollider = GetComponent<Collider2D>();
        playerLife = 100;

        healthBar.SetMaxHealth(playerMaxLife);

        sceneLoader = FindObjectOfType<SceneLoader>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        jump();
        IsJumping();
        
    }

    private void jump()
    {
        if(!myCollider.IsTouchingLayers(LayerMask.GetMask("Platform")))
        {
            return;
        }
        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            Vector2 jumpVelocity = new Vector2(0f, jumpForce);
            myRB.velocity = jumpVelocity;
        }
    }

    private void IsJumping()
    {
        if (!myCollider.IsTouchingLayers(LayerMask.GetMask("Platform")) && !myCollider.IsTouchingLayers(LayerMask.GetMask("Floor")))
        {
            myAnimator.SetBool("InAir", true);
        }
        else
        {
            myAnimator.SetBool("InAir", false);
        }
    }

    private void Move()
    {
        float controlFlow = CrossPlatformInputManager.GetAxis("Horizontal"); //-1 to +1
        Vector2 playerVelocity = new Vector2(controlFlow * moveSpeed, myRB.velocity.y);
        myRB.velocity = playerVelocity;

        bool playerHorizontalVelocity = Mathf.Abs(myRB.velocity.x)>Mathf.Epsilon;
        myAnimator.SetBool("isRunning", playerHorizontalVelocity);
        

        if(controlFlow<0 && facingRight)
        {
            Flip();
        }
        else if(controlFlow>0 && !facingRight)
        {
            Flip();
        }
    }


    void Flip()
    {

        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);

    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Floor")
        {
            PlayerHit(100);
        }
    }

    public void PlayerHit(int damage)
    {
        playerLife -= damage;

        healthBar.SetHealth(playerLife);

        if(playerLife <= 0)
        {
            PlayerDeath();
        }

        myAnimator.SetTrigger("isHurt");
    }

    private void PlayerDeath()
    {
        
        myAnimator.SetTrigger("isDead");
        Destroy(gameObject, 0.5f);

        sceneLoader.LoadDeadScene();
    }

   
}
