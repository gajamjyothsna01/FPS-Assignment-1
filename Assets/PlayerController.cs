using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public AudioClip audioClip;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();  
    }

    // Update is called once per frame
    void Update()
    {

       /* if (Input.GetKeyDown(KeyCode.F))
        {
            animator.SetBool("", !animator.GetBool("IsAiming"));
        }*/
        if (Input.GetMouseButton(0))
        {
            /*if (ammo > 0)
            {
                // animator.SetBool("isFiring", !animator.GetBool("isFiring"));
                
                
                //WhenZombieeGotHit();
                ammo = Mathf.Clamp(ammo - 10, 0, maxAmmo);
                // Debug.Log(ammo);
            }
            else
            {
                //Trigger the sound for empty bullets.
            }*/
            animator.SetTrigger("isFiring");

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

}
