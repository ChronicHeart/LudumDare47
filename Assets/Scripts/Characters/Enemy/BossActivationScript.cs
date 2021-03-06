﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossActivationScript : MonoBehaviour
{
    [SerializeField]
    BossController boss;            // The boss that we will activate
    [SerializeField]
    GameObject door;                // The door that closes behind the player
    [SerializeField]
    AudioSource sceneAudio;
    [SerializeField]
    AudioClip bossMusic;

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
            sceneAudio.Stop();
            sceneAudio.clip = bossMusic;
            sceneAudio.Play();
            sceneAudio.volume = 0.35f;
        }
    }
}
