using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Controls : NetworkBehaviour
{

    Animator anim;
    NetworkAnimator netAnim;
    public GameObject bulletPrefab;
    float speed;
    bool isArmDead = false;


    // Use this for initialization
    void Start()
    {
        anim = GameObject.FindObjectOfType<Animator>();
        netAnim = GameObject.FindObjectOfType<NetworkAnimator>();
    }


    // Update is called once per frame
    void Update()
    {
        //Reset isHit so it doesn't loop
        anim.SetBool("isHit", false);

        //Checks for Key inputs
        //Changing the float value of Speed changes the value in the blend tree 
        if (Input.GetKey(KeyCode.W))
        {
            anim.SetBool("isRunning", true);
            anim.SetFloat("Speed", 0.5f);

            if (Input.GetKey(KeyCode.A))
            {
                anim.SetFloat("Speed", 0.25f);
            } else if (Input.GetKey(KeyCode.D))
            {
                anim.SetFloat("Speed", 0.75f);
            }
        } else if (Input.GetKey(KeyCode.A))
        {
            anim.SetFloat("Speed", 0.0f);
            anim.SetBool("isRunning", true);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            anim.SetFloat("Speed", 1.0f);
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }


        //Starts the casting animation
        //An event is tied to this animation calling the fire() method
        //roughly when the players hand is halfway swung
        if (Input.GetMouseButtonDown(0))
        {
            netAnim.SetTrigger("attacking");
        }

    }


    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "ball")
        {
            netAnim.SetTrigger("hit");
            isArmDead = true;
        }
    }


    //Added as when calling CmdFire() from animation event it was not recogning the [command] 
    //and therefore not working across the network
    void fire()
    {
        CmdFire();
    }


    //Method for instantiating and throwing the ball on over the network
    [Command]
    void CmdFire()
    {
        if (!isArmDead)
        {
            GameObject bullet = (GameObject)Instantiate(bulletPrefab, this.transform.position + transform.right * 1 + transform.up * 1, this.transform.rotation);
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 10.0f;
            NetworkServer.Spawn(bullet);

            Destroy(bullet, 2);
        }
    }

    public void FootR() { }
    public void FootL() { }
}
