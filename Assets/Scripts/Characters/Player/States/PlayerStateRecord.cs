using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateRecord : PlayerStateBase
{
    public override void EnterState(PlayerController player)
    {
        // Make the weapon visible and switch animation states
        player.recordHeld.SetActive(true);
        //player.myAnimator.ResetTrigger("exitState");
        player.myAnimator.SetTrigger("record");
    }

    public override void ExitState(PlayerController player)
    {
        // Hide the weapon and make sure we don't return to this animation state
        player.recordHeld.SetActive(false);
        player.myAnimator.ResetTrigger("record");
        player.myAnimator.SetTrigger("exitState");
    }

    public override void Update(PlayerController player)
    {
        if (Input.GetButtonDown("Fire1"))
        {
            WeaponAttack(player);
        }
    }

    public override void WeaponAttack(PlayerController player)
    {
        // Do the animation
        player.myAnimator.SetTrigger("isAttacking");
    }
}
