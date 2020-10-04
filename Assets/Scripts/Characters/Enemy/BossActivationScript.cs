using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossActivationScript : MonoBehaviour
{
    [SerializeField]
    BossController boss;            // The boss that we will activate

    private void Start()
    {
        boss.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            boss.gameObject.SetActive(true);
        }
    }
}
