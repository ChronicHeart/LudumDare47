using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : CharacterHealth
{
    #region Events and Delegates
    public delegate void HealthChanged();
    public event HealthChanged PlayerHealthChanged;

    #endregion


    public override void Die()
    {
        // Insert functionality for player death here
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
