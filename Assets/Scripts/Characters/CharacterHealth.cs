﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Collider))]
public class CharacterHealth : MonoBehaviour, IDamageable<int>
{
    #region Health

    [Header("Health")]

    [SerializeField]
    protected int maxHP = 10;                                              // The maximum HP this object can start with
    protected int m_currentHP;
    public int MaxHP
    {
        get { return maxHP; }
    }
    public int CurrentHP                                                // The accessor for current health. Use this instead of calling health directly
    {
        get { return m_currentHP; }
        set { m_currentHP = Mathf.Clamp(value, 0, maxHP); }
    }

    #endregion

    [Header("Audio")]
    [SerializeField]
    protected AudioClip sfxHurt;
    [SerializeField]
    protected AudioClip sfxDeath;

    [Header("UI Canvas")]
    [SerializeField]
    GameObject playerUICanvas;
    [SerializeField]
    GameObject winScreenCanvas;

    #region Components and References

    protected Collider myCollider;                                    // The collider of this enemy

    [SerializeField]
    protected GameObject myRenderer;                                // The visual display of this enemy 
    protected Animator animator;
    protected AudioSource audioSource;

    #endregion

    protected virtual void Awake()
    {
        // Initialize health
        CurrentHP = maxHP;

        // Set references and components
        myCollider = GetComponent<Collider>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        if (myRenderer == null)
        {
            Debug.LogWarning(name + ": renderer has not been assigned!");
        }
    }

    public virtual void Die()
    {
        // Play death sfx
        audioSource.PlayOneShot(sfxDeath);

        // Disable the renderer and collider so that the enemy can no longer be interacted with
        myRenderer.SetActive(false);
        myCollider.enabled = false;
        StartCoroutine(DeathDelay());
        
    }

    public virtual void HealHP(int hpRestored)
    {
        // Restore HP based on the parameters given
        CurrentHP += hpRestored;
    }

    public virtual void Revive()
    {
        // Reset HP
        CurrentHP = maxHP;

        // Reenable the renderer and collider so that the gameobject can be interacted with again. 
        myRenderer.SetActive(true);
        myCollider.enabled = true;
    }

    public virtual void TakeDamage(int damageTaken)
    {
        // Play sfx
        audioSource.PlayOneShot(sfxHurt);

        // Take damage based on the given parameter
        CurrentHP -= damageTaken;

        // Kill the enemy if they have no HP
        if (CurrentHP <= 0)
        {
            Die();
        }

        //Debug.Log(name + ": Current HP is at " + CurrentHP);
    }

    IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(0.5f);

        if (tag == "Boss")
        {
            Time.timeScale = 0;
            playerUICanvas.SetActive(false);
            winScreenCanvas.SetActive(true);
            
            Destroy(gameObject);
        }

        if (tag == "Enemy")
        {
            Destroy(gameObject);
        }
    }
}
