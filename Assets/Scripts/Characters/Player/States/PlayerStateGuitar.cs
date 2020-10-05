﻿using UnityEngine;

public class PlayerStateGuitar : PlayerStateBase
{
    public override void EnterState(PlayerController player)
    {
        player.guitar.SetActive(true);
        player.myAnimator.SetTrigger("guitar");
    }

    public override void ExitState(PlayerController player)
    {
        player.guitar.SetActive(false);
        player.myAnimator.ResetTrigger("guitar");
        player.myAnimator.SetTrigger("exitState");
    }

    public override void Update(PlayerController player)
    {
        WeaponAttack(player);
    }

    public override void WeaponAttack(PlayerController player)
    {
        if (Input.GetButtonDown("Fire1"))
        {
            player.guitarHitBox.enabled = true;
            player.myAnimator.SetTrigger("isAttacking");

            // Player the partical system
            ParticleSystem particleSystem = player.guitar.GetComponentInChildren<ParticleSystem>();
            if (particleSystem != null)
                particleSystem.Play();
            else
                Debug.LogWarning("Couldn't find the particle system on the guitar");
        }
    }
}
