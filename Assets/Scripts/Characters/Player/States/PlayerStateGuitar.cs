using UnityEngine;

public class PlayerStateGuitar : PlayerStateBase
{
    public override void EnterState(PlayerController player)
    {
        player.guitar.SetActive(true);
    }

    public override void ExitState(PlayerController player)
    {
        player.guitar.SetActive(false);
    }

    public override void Update(PlayerController player)
    {
        WeaponAttack(player);
    }

    public override void WeaponAttack(PlayerController player)
    {
        if (Input.GetMouseButtonDown(0))
        {
            player.guitarHitBox.enabled = true;
            player.myAnimator.SetTrigger("isAttacking");
        }
    }
}
