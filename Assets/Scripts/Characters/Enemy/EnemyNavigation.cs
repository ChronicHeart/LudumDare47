using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class EnemyNavigation : MonoBehaviour
{
    [Tooltip("Array used for containing patrol points an enemy will walk between")]
    [SerializeField] GameObject[] patrolPoints;

    int curPoint = 0;
    int attackPower = 5;
    bool reverseCourse = false;
    bool playerDetected = false;    

    public NavMeshAgent agent;
    public GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        if (agent == null)
        {
            Debug.LogWarning(name + " does not have a NavMeshAgent component.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerDetected)
        {
            agent.SetDestination(patrolPoints[curPoint].transform.position);            
        }
        else
        {
            agent.SetDestination(player.transform.position);            
        }

        RaycastHit hit;
        Ray ray = new Ray(transform.position + new Vector3(0, 1, 0), transform.forward * 4);
        Debug.DrawRay(transform.position + new Vector3(0, 1, 0), transform.forward * 4, Color.red);

        if (Physics.Raycast(ray, out hit, 4))
        {
            if (hit.collider.tag == "Player")
            {
                playerDetected = true;
                StopCoroutine("Delay");
                StartCoroutine("Delay");
            }           
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PatrolPoint")
        {
            if (reverseCourse)
            {
                curPoint -= 1;
                if (curPoint <= 0)
                {
                    reverseCourse = false;
                }
            }
            else if (curPoint >= patrolPoints.Length - 1)
            {
                curPoint -= 1;
                reverseCourse = true;
            }
            else
            {
                curPoint += 1;
            }
        }
        
        /*if (other.tag == "Player")
        {
            other.SendMessage("TakeDamage", attackPower, SendMessageOptions.DontRequireReceiver);
            Debug.Log("Player has taken " + attackPower + " damage.");
        }*/
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.SendMessage("TakeDamage", attackPower, SendMessageOptions.DontRequireReceiver);
            Debug.Log("Player has taken " + attackPower + " damage.");
        }
    }*/

    IEnumerator Delay()
    {        
        //Debug.Log("Coroutine has started.");
        yield return new WaitForSeconds(3.0f);
        playerDetected = false;        
    }
    
}
