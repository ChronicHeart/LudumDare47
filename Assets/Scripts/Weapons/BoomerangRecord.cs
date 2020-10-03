using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Primary Author: Lena Taylor
/// Date Started: 10/3/2020
/// Purpose: To control the boomerang's movement and 
/// </summary>
public class BoomerangRecord : MonoBehaviour
{
    [SerializeField]
    int attackPower = 5;
    [SerializeField]
    float speed = 50;
    [SerializeField]
    float stayTime = .5f;           // The time in seconds that the boomerang stays at it's target location

    Vector3 target;                 // Where the boomerang is being thrown
    PlayerController player;              // Reference to the player
    bool targetReached;             // Has the boomerang reached it's target destination? 
    AudioSource audioSource;

    public void Awake()
    {
        // Set target position and reference to player
        target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        target = new Vector3(target.x, 1, target.z);
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        audioSource = GetComponent<AudioSource>();

        StartCoroutine(FindTargetPosition());
    }

    IEnumerator FindTargetPosition()
    {
        // Do nothing until we're near the target

        while(Vector3.Distance(transform.position, target) > .2)
        {
            yield return null;
        }

        // Now that we're near the target, wait for the specified amount of time
        yield return new WaitForSeconds(stayTime);

        // Set targetReached to true to show that we reached our destination. 
        targetReached = true;
    }

    public void Update()
    {
        // Move the boomerang based on whether or not it's reached it's target
        if (!targetReached)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z), speed * Time.deltaTime);

            // If we're close enough to the player, destroy this object and reveal the boomerang in the player's hand
            if(Vector3.Distance(transform.position, player.transform.position) < .2f)
            {
                player.playerStateRecord.CatchRecord();
                Destroy(gameObject);
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        // If other is not the player, do damage
        if(other.gameObject != player.gameObject)
        {
            other.SendMessage("TakeDamage", attackPower,SendMessageOptions.DontRequireReceiver);
            audioSource.Play();
        }
    }
}
