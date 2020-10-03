using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateTrumpet : PlayerStateBase
{
    public override void EnterState(PlayerController player)
    {
        player.trumpet.SetActive(true);
        player.myAnimator.SetTrigger("trumpet");
    }

    public override void ExitState(PlayerController player)
    {
        player.trumpet.SetActive(false);
        player.myAnimator.ResetTrigger("trumpet");
        player.myAnimator.SetTrigger("exitState");
    }

    public override void Update(PlayerController player)
    {
        if(Input.GetButtonDown("Fire1"))
            WeaponAttack(player);
    }

    public override void WeaponAttack(PlayerController player)
    {
        player.myAnimator.SetTrigger("isAttacking");
        return;
    }
}
