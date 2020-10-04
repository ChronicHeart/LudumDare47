using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossController : MonoBehaviour
{
    #region Attack Variables

    [Header("Attack Variables")]
    [SerializeField]
    GameObject bulletPrefab;                     // The bullet to be  spawned
    [SerializeField]
    int bulletAmount;                            // How many bullets will spawn when the trumpet shoots
    [SerializeField]
    int bulletAttackPower;                       // How much damage the bullets will do
    [SerializeField]
    int bulletSpeed;                             // How quickly the bullet moves
    [SerializeField]
    float fireRate = 1f;                         // Minimum seconds between blasts
    [SerializeField]
    float spread;                                // The angle of the blast
    [SerializeField]
    AudioClip sfxTrumpetFire;                   // The sound effect that plays when the boss attacks

    #endregion

    #region Components and References

    NavMeshAgent navMeshAgent;
    Animator myAnimator;
    GameObject player;
    AudioSource audioSource;
    [SerializeField]
    Transform bulletSpawnPoint;             // Where the bullets will spawn

    float timer;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // Set references
        navMeshAgent = GetComponent<NavMeshAgent>();
        myAnimator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");

        timer = fireRate;
    }

    void Update()
    {
        // Move towareds the player
        navMeshAgent.SetDestination(player.transform.position);

        //Update animation
        Vector3 speed = navMeshAgent.velocity.normalized;
        speed = transform.InverseTransformDirection(speed);

        myAnimator.SetFloat("speedX", speed.x);
        myAnimator.SetFloat("speedZ", speed.z);

        // Fire if timer equals zero
        timer -= Time.deltaTime;
        if(timer < 0)
        {
            FireWeapon();
            timer = fireRate;
        }
    }

    void FireWeapon()
    {
        // Play sound effect
        audioSource.PlayOneShot(sfxTrumpetFire);

        // Spawn all the bullets we need and store them into a array
        //List<GameObject>  bullets = new List<GameObject>(player.bulletAmount);
        GameObject[] bullets = new GameObject[bulletAmount];

        for (int i = 0; i < bullets.Length; i++)
        {
            Vector3 origin = new Vector3(bulletSpawnPoint.transform.position.x, bulletSpawnPoint.transform.position.y - 3, bulletSpawnPoint.transform.position.z);
            GameObject bullet = GameObject.Instantiate(bulletPrefab, origin, Quaternion.identity);

            //bullets.Add(bullet);
            bullets[i] = bullet;

            bullet.SetActive(true);

            // Rotate the bullets to face a new direction based on the spread
            // Also be sure to set the speed and attack power of the bullets

            BulletScript bulletScript = bullet.GetComponent<BulletScript>();

            bulletScript.attackPower = bulletAttackPower;
            bulletScript.speed = bulletSpeed;

            float playerAngle = Vector3.SignedAngle(transform.forward, Vector3.forward, Vector3.up);
            bullet.transform.rotation = Quaternion.AngleAxis(-playerAngle + (-spread / 2 + (i * (spread / bullets.Length))), Vector3.up);

            // Reset the timer

        }
    }

    /*
    IEnumerator FireWeapon()
    {
        Debug.Log("FireWeaponCalled");

        yield return new WaitForSeconds(fireRate);

        // Play sound effect
        audioSource.PlayOneShot(sfxTrumpetFire);

        // Spawn all the bullets we need and store them into a array
        //List<GameObject>  bullets = new List<GameObject>(player.bulletAmount);
        GameObject[] bullets = new GameObject[bulletAmount];

        for (int i = 0; i < bullets.Length; i++)
        {
            GameObject bullet = GameObject.Instantiate(bulletPrefab, bulletSpawnPoint.transform.position, Quaternion.identity);

            //bullets.Add(bullet);
            bullets[i] = bullet;

            bullet.SetActive(true);

            // Rotate the bullets to face a new direction based on the spread
            // Also be sure to set the speed and attack power of the bullets

            BulletScript bulletScript = bullet.GetComponent<BulletScript>();

            bulletScript.attackPower = bulletAttackPower;
            bulletScript.speed = bulletSpeed;

            float playerAngle = Vector3.SignedAngle(transform.forward, Vector3.forward, Vector3.up);
            bullet.transform.rotation = Quaternion.AngleAxis(-playerAngle + (-spread / 2 + (i * (spread / bullets.Length))), Vector3.up);

            // Reset the timer
            StartCoroutine(FireWeapon());
        }

    }*/

    // Update is called once per frame

}
