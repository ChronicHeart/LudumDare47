using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerStateTrumpet : PlayerStateBase
{
    bool canFire = true;
    float timer = 0;

    public override void EnterState(PlayerController player)
    {
        player.trumpet.SetActive(true);
        player.myAnimator.SetTrigger("trumpet");
        canFire = true;
        timer = player.fireRate;
    }

    public override void ExitState(PlayerController player)
    {
        player.trumpet.SetActive(false);
        player.myAnimator.ResetTrigger("trumpet");
        player.myAnimator.SetTrigger("exitState");
        canFire = false;
    }

    public override void Update(PlayerController player)
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            canFire = true;
        }

        if(Input.GetButtonDown("Fire1"))
            WeaponAttack(player);
    }

    public override void WeaponAttack(PlayerController player)
    {
        // Set the animation trigger
        player.myAnimator.SetTrigger("isAttacking");

        // Check to see if we can fire
        if (canFire)
        {
            // Make sure we can't run this code a second time
            canFire = false;

            // Play sound effect
            player.audioSource.PlayOneShot(player.sfxTrumpetFire);

            // Spawn all the bullets we need and store them into a array
            //List<GameObject>  bullets = new List<GameObject>(player.bulletAmount);
            GameObject[] bullets = new GameObject[player.bulletAmount];

            for (int i = 0; i < bullets.Length; i++)
            {
                GameObject bullet = GameObject.Instantiate(player.bulletPrefab, player.socket.transform.position, Quaternion.identity);

                //bullets.Add(bullet);
                bullets[i] = bullet;

                bullet.SetActive(true);

                // Rotate the bullets to face a new direction based on the spread
                // Also be sure to set the speed and attack power of the bullets

                BulletScript bulletScript = bullet.GetComponent<BulletScript>();

                bulletScript.attackPower = player.bulletAttackPower;
                bulletScript.speed = player.bulletSpeed;

                float playerAngle = Vector3.SignedAngle(player.transform.forward, Vector3.forward, Vector3.up);
                bullet.transform.rotation = Quaternion.AngleAxis(-playerAngle + (-player.spread / 2 + (i * (player.spread / bullets.Length))), Vector3.up);

                // Reset the timer
                timer = player.fireRate;
            }
        }
        
    }
}
