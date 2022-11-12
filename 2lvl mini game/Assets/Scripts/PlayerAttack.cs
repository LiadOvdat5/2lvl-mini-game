using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityStandardAssets.CrossPlatformInput;

public class PlayerAttack : MonoBehaviour
{
    public GameObject fireBallPrefab;
    public Transform firePoint;

   

    Animator myAnimator;


    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Attack();
        }
    }

    private void Attack()
    {

        myAnimator.SetTrigger("isAttacking");

        //instensiate fire prefab with force!
        GameObject fireBall = Instantiate(fireBallPrefab, firePoint.position, firePoint.rotation);

    }
}
