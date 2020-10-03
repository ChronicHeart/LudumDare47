using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Lena Taylor
/// Date: 10/2/2020
/// Purpose: To damage any damageable object that enters this trigger volume
/// </summary>
public class DamageHitbox : MonoBehaviour
{
    public int attackPower = 5;         // How much damage will this object do?
    public AudioClip sfxHit;

    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Make sure we don't damage the parent
        if (other.gameObject == transform.parent)
            return;

        // Damage the other object
        IDamageable<int> otherHealth = other.GetComponent<IDamageable<int>>();
        if (otherHealth != null)
        {
            otherHealth.TakeDamage(attackPower);

            // Play a sound

            if (audioSource != null && sfxHit != null && audioSource.isPlaying == false)
            {
                audioSource.Play();
            }
        }


    }
}
