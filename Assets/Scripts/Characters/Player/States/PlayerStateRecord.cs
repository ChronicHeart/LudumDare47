using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateRecord : PlayerStateBase
{
    public BoomerangRecord boomerang;           // Reference to the boomerang we'll throw

    PlayerController playerRef;                 // Reference to the player for methods unique to this class

    public override void EnterState(PlayerController player)
    {
        // Make the weapon visible and switch animation states
        player.recordHeld.SetActive(true);
        //player.myAnimator.ResetTrigger("exitState");
        player.myAnimator.SetTrigger("record");

        // Set a reference to the player for use methods unique to this class
        playerRef = player;
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
        if (player.recordHeld.activeInHierarchy)
        {
            // Disable the record the player's holding
            player.recordHeld.SetActive(false);

            // Spawn the boomerang
            boomerang = GameObject.Instantiate(player.recordToThrow, player.socket.position, Quaternion.identity);

            // Do the animation
            player.myAnimator.SetTrigger("isAttacking");
        }
    }

    public void CatchRecord()
    {
        // Make the record reappear in the player's hand
        playerRef.recordHeld.SetActive(true);

        // Destroy the boomerang if it isn't already gone
        if (boomerang != null)
            GameObject.Destroy(boomerang);
    }
}
