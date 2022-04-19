using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    Animator animator;
    NavMeshAgent agent;
    public GameObject target;
    public float walkingSpeed;
    public float runningSpeed;
    
    enum STATE { IDLE, WALK, RUN, FIRE,DIE};
    STATE state = STATE.IDLE;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null && GameStart.isGameOver == false)
        {
            target = GameObject.FindGameObjectWithTag("Player");
            return;
        }
        switch (state)
        {
            case STATE.IDLE:
                 if(SeeTheplayer())
                {
                    state = STATE.RUN;
                }
                else if(Random.Range(0,1000) < 5)
                {
                    state = STATE.WALK;
                }
                break;
            case STATE.WALK:
                if (!agent.hasPath)
                {
                    float randValueX = transform.position.x + Random.Range(-5f, 5f);
                    float randValueZ = transform.position.z + Random.Range(-5f, 5f);
                    float ValueY = Terrain.activeTerrain.SampleHeight(new Vector3(randValueX, 0f, randValueZ));
                    Vector3 destination = new Vector3(randValueX, ValueY, randValueZ);
                    agent.SetDestination(destination);
                    agent.stoppingDistance = 0f;
                    agent.speed = walkingSpeed;
                    TurnOffAllTriggerAnim();
                    animator.SetBool("isWalking", true);
                }
                if (SeeTheplayer())
                {
                    state = STATE.RUN;
                }
                else if (Random.Range(0, 1000) < 7)
                {
                    state = STATE.IDLE;
                    TurnOffAllTriggerAnim();
                    agent.ResetPath();
                }

                break;

            case STATE.RUN:
                if (GameStart.isGameOver)
                {
                    TurnOffAllTriggerAnim();
                    state = STATE.WALK;
                    return;
                }
                agent.SetDestination(target.transform.position);
                agent.stoppingDistance = 2f;
                TurnOffAllTriggerAnim();
                animator.SetBool("isRunning", true);
                agent.speed = runningSpeed;
                if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
                {
                    state = STATE.FIRE;
                }
                if (SeeTheplayer())
                {
                    state = STATE.WALK;
                    agent.ResetPath();
                }


                break;

            case STATE.FIRE:
                if (GameStart.isGameOver)
                {
                    TurnOffAllTriggerAnim();
                    state = STATE.WALK;
                    return;
                }
                TurnOffAllTriggerAnim();
                animator.SetBool("isFiring", true);
                transform.LookAt(target.transform.position);//Zombies should look at Player
                if (DistanceBetweenPlayer() > agent.stoppingDistance + 2)
                {
                    state = STATE.RUN;
                }
                print("Firing State");
                break;
            case STATE.DIE:
                break;
            default:
                break;
        }
    }
    public void TurnOffAllTriggerAnim()//All animation are off
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);
        animator.SetBool("isFiring", false);
        animator.SetBool("isReload", false);
        animator.SetBool("isDead", false);
    }

    private bool SeeTheplayer()
    {
        if(DistanceBetweenPlayer() < 5)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private float DistanceBetweenPlayer()
    {
       if(GameStart.isGameOver)
        {
            return Mathf.Infinity;
        }
        return Vector3.Distance(target.transform.position, this.transform.position);
    }


}
//Checking for Game Over or not.
public class GameStart
{
    public static bool isGameOver = false;

}
