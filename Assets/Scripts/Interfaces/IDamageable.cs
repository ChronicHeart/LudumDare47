using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable<T>
{
    void TakeDamage(T damageTaken);

    void HealHP(T hpRestored);

    void Die();

    void Revive();
}
