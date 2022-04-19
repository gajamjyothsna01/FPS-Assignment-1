using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    Animator animator;
    public AudioSource audioSource;
    public AudioClip clip;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();   
        audioSource = GetComponent<AudioSource>();
        clip = GetComponent<AudioClip>();
        

         animator.SetBool("isIdle", true);
   }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
           

        }
        if(Input.GetKeyDown(KeyCode.J))
        {
            animator.SetBool("isWalking", true);
            audioSource.Play();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("isFiring");
            audioSource.Play();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            animator.SetBool("isReload",true);
        }




    }
}
