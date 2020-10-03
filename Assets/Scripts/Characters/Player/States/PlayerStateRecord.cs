using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateRecord : PlayerStateBase
{
    public override void EnterState(PlayerController player)
    {
        player.recordHeld.SetActive(true);
    }

    public override void ExitState(PlayerController player)
    {
        player.recordHeld.SetActive(false);
    }

    public override void Update(PlayerController player)
    {
        return;
    }

    public override void WeaponAttack(PlayerController player)
    {
        return;
    }
}
