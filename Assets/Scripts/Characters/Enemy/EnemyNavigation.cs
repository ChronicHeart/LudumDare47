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
    bool reverseCourse = false;

    public NavMeshAgent agent;


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
        agent.SetDestination(patrolPoints[curPoint].transform.position);
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
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "PatrolPoint")
        {
            curPoint += 1;
            if (curPoint >= patrolPoints.Length)
            {
                curPoint = 0;
            }
        }
    }*/
}
