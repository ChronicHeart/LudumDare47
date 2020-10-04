using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : CharacterHealth
{
    #region Events and Delegates
    public delegate void HealthChanged();
    public event HealthChanged PlayerHealthChanged;
    #endregion

    PlayerController playerController;

    bool isDying = false;

    protected override void Awake()
    {
        base.Awake();
        playerController = GetComponent<PlayerController>();
        isDying = false;
    }
    public override void Die()
    {
        // Use a bool to ensure this function cannot be called repeatedly
        if (isDying == true)
            return;
        isDying = true;

        // Play sfx
        audioSource.PlayOneShot(sfxDeath);

        // Insert functionality for player death here
        animator.SetTrigger("exitState");
        animator.SetTrigger("isDead");
        // Disable player movement
        playerController.StopAllCoroutines();
        playerController.enabled = false;
    }

    public void ResetScene()
    {
        Scene thisScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(thisScene.buildIndex);
    }
    

    public override void Revive()
    {
        // Insert functionality for the player respawning here, assuming it's needed
    }

    public override void HealHP(int hpRestored)
    {
        base.HealHP(hpRestored);

        // Call the player health changed event to let other game objects know
        // that this value has changed
        PlayerHealthChanged();
    }

    public override void TakeDamage(int damageTaken)
    {
        base.TakeDamage(damageTaken);

        // Call the player health changed event to let other game objects know
        // that this value has changed
        PlayerHealthChanged();
    }
}
