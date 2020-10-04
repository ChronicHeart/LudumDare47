using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossActivationScript : MonoBehaviour
{
    [SerializeField]
    BossController boss;            // The boss that we will activate
    [SerializeField]
    GameObject door;                // The door that closes behind the player

    private void Start()
    {
        boss.gameObject.SetActive(false);
        door.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            boss.gameObject.SetActive(true);
            door.SetActive(true);
        }
    }
}
