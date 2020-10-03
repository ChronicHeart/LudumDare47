using UnityEngine;
/// <summary>
/// Author: Lena Taylor
/// Date: 10/3/2020
/// Purpose: To serve as the parent class for all the possible states the player can be in. This class itself should not
///          be attached to any gameobject, nor should any of it's child classes. Instead, readonly fields of them will be present
///          in the player, and their methods should be called from there. 
/// </summary>
public abstract class PlayerStateBase
{
    public abstract void EnterState(PlayerController player);

    public abstract void ExitState(PlayerController player);

    public abstract void Update(PlayerController player);

    public abstract void WeaponAttack(PlayerController player);
}
