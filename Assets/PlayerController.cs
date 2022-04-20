using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    public float playerSpeed;
    public float playerJumpForce;
    public float rotationSpeed;
    Quaternion playerRotataion, camRotation;
    public Camera cam;
    float inputX, inputZ;
    public float minX = -90;
    public float maxX = 90;
    CapsuleCollider capsuleCollider;
    public Animator animator;
    int ammo = 100;
    int maxAmmo = 100;
    public Transform bulletLaunchPosition;
    int health = 100;
    int maxHealth = 100;
   // public SpriteRenderer sprite;
   // public bool isGameWin = false;
    //public GameObject target;
    // public AudioClip audioClip;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();  
        //animator = GetComponent<Animator>();
       
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("isWalking", true);
        /* if (Input.GetKeyDown(KeyCode.F))
         {
             animator.SetBool("", !animator.GetBool("IsAiming"));
         }*/
        if (Input.GetMouseButtonDown(0))
        {
             // animator.SetBool("isFiring", !animator.GetBool("isFiring"));
                Debug.Log("Firing State");
                animator.SetTrigger("isFiring");
                Debug.Log("Player Hit Method");
                Debug.Log("Ammo Firing" +ammo);
                ammo--;
                
                WhenPlayerHitEnemy();
                //audioSource.Play();
               

               // ammo = Mathf.Clamp(ammo - 10, 0, maxAmmo);
                // Debug.Log(ammo);
            


        }
    }

    private void WhenPlayerHitEnemy()
    {
        RaycastHit hitInformation;
        Debug.Log("Got the Ray Info");
        if (Physics.Raycast(bulletLaunchPosition.position, bulletLaunchPosition.forward, out hitInformation, 100f))
        {
            Debug.Log("Bulleted Hit the enemy");

            GameObject hitEnenimes = hitInformation.collider.gameObject;
            if (hitEnenimes.tag == "Enemy")
            {
                Debug.Log("Enemy Found");
                hitEnenimes.GetComponent<EnemyController>().DeadEnemy();
                Debug.Log("Going to Dead state in EC");
            }
        }
    }

    private void FixedUpdate()
    {
        inputX = Input.GetAxis("Horizontal");
        inputZ = Input.GetAxis("Vertical");
        transform.position = transform.position + new Vector3(inputX * playerSpeed, 0, inputZ * playerSpeed);

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.AddForce(Vector3.up * playerJumpForce);
        }
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;



        playerRotataion = Quaternion.Euler(0, mouseY, 0) * playerRotataion; // rOTATION IN X
                                                                            // Debug.Log(playerRotataion);
        camRotation = ClampRotationOfPlayer(Quaternion.Euler(-mouseX, 0, 0) * camRotation);//Rotation in y
        //Debug.Log("camRotation" + camRotation);
        this.transform.localRotation = playerRotataion;
        cam.transform.localRotation = camRotation;
        //playerRotataion = transform.localRotation;



    }
    Quaternion ClampRotationOfPlayer(Quaternion n) //clamp - restricts the player rotation by maximum and minimumv value.
    {
        n.w = 1f;
        n.x /= n.w;
        n.y /= n.w;
        n.z /= n.w;
        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(n.x);
        angleX = Mathf.Clamp(angleX, minX, maxX);
        n.x = Mathf.Tan(Mathf.Deg2Rad * 0.5f * angleX);
        return n;

    }
    bool IsGrounded()
    {
        RaycastHit rayCasthit;
        if (Physics.SphereCast(transform.position, capsuleCollider.radius, Vector3.down, out rayCasthit, (capsuleCollider.height / 2) - capsuleCollider.radius + 0.1f))
        {
            return true;

        }
        else
        {
            return false;
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ammo" && ammo < maxAmmo)
        {
            Debug.Log("Near to Ammo");
            collision.gameObject.SetActive(false);
            Debug.Log("Collected Ammo Box");
            // ammo = ammo + 10;
            ammo = Mathf.Clamp(ammo + 10, 0, maxAmmo);
            Debug.Log("Ammo" + ammo);
        }
        if (collision.gameObject.tag == "Health" && health < maxHealth)
        {
            Debug.Log("Near to Health");
            collision.gameObject.SetActive(false);
            Debug.Log("Collected Health Box");
            // medical = medical + 10;
            health = Mathf.Clamp(health + 10, 0, maxHealth);


        }
    }

        public void HittingPlayer(float hittingValue)
    {
        //hitting value is the Value of health that to be decreased.

        health = (int)(Mathf.Clamp(health - hittingValue, 0, maxHealth));
        Debug.Log("Health" +health);
        if(health <=0)
        {
            Debug.Log("GameOver");
            Debug.Log("Player is Dead");
            Destroy(this.gameObject);
           GameObject.Find("PBRCharacter 1").GetComponent<EnemyController>().TurnOffAllTriggerAnim();
           // sprite.enabled = true;
           // isGameWin = true;

        }
    }
   
}
