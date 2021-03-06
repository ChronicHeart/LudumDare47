﻿using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    #region Delegates and Events
    public delegate void States();
    public event States PlayerStateChanged;                     // This event is fired whenever the player changes states

    #endregion

    #region Enums
    //public enum PlayerDirection { Right = 1, Left = -1};
    #endregion

    #region Movement Variables

    [Header("Movement Variables")]
    [SerializeField]
    float speed = 5f;
    [SerializeField]
    float jumpForce = 1f;
    [SerializeField]
    float gravity = -9.18f;
    [SerializeField]
    float gravityWhileFalling = 10f;

    private Vector3 playerVelocity;                    // The current velocity of the player. We feed this into myCC for movement
    private bool groundedPlayer;

    LayerMask layerMask;

    //public PlayerDirection direction = PlayerDirection.Right;

    #endregion

    #region Trumpet Variables

    [Header("Trumpet")]
    public GameObject bulletPrefab;                     // The bullet to be  spawned
    public int bulletAmount;                            // How many bullets will spawn when the trumpet shoots
    public int bulletAttackPower;                       // How much damage the bullets will do
    public int bulletSpeed;                             // How quickly the bullet moves
    public float fireRate = 1f;                         // Minimum seconds between blasts
    public float spread;                                // The angle of the blast

    #endregion

    #region Audio
    [Header("Audio Clips")]
    public AudioClip sfxSwitchWarning;              // Plays right before switching states
    public AudioClip sfxOnSwitch;                   // Plays upon switching states
    public AudioClip sfxTrumpetFire;                // Plays when the trumpet fires

    #endregion

    #region Components and References

    [Header("References")]
    public Transform socket;                        // The empty game object that holds weapons
    public Collider guitarHitBox;                    // The range of the guitar attack. 
    public GameObject guitar;                        // The guitar the player holds
    public GameObject recordHeld;                    // The record the player holds
    public BoomerangRecord recordToThrow;            // The gameobject that will be instantiated and thrown by the player
    public GameObject trumpet;                      // The trumpet to be held by the player
    public Transform bulletSpawnPoint;              // The place where all bullets spawn from

    Camera mainCamera;

    [HideInInspector]
    public CharacterController myCC;                 // The character controller on this object
    [HideInInspector]
    public Transform myTransform;
    [HideInInspector]
    public Animator myAnimator;
    [HideInInspector]
    public AudioSource audioSource;

    #endregion

    #region Player States

    [Header("States")]
    public float switchStateSeconds;                    // How long it takes us to switch states

    private PlayerStateBase currentState;               // The current state the player is in
    private PlayerStateBase lastState;                  // The previous state the player was in

    // ----- All of the players possible states

    public readonly PlayerStateGuitar playerStateGuitar = new PlayerStateGuitar();
    public readonly PlayerStateRecord playerStateRecord = new PlayerStateRecord();
    public readonly PlayerStateTrumpet playerStateTrumpet = new PlayerStateTrumpet();

    // ----- An array that contains all of the states for the sake of cycling between them
    [HideInInspector]
    public  PlayerStateBase[] allStates;
    private int currentStateIndex = 0;              // Where we currently are in the arry
    public int CurrentStateIndex { get { return currentStateIndex; } }

    // ------ Accessors for the current and last state
    public PlayerStateBase CurrentState { get { return currentState;} }
    public PlayerStateBase LastState { get { return lastState;} }

    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        // Initilize the state array
        currentStateIndex = 0;
        allStates = new PlayerStateBase[3];
        allStates[0] = playerStateGuitar;
        allStates[1] = playerStateRecord;
        allStates[2] = playerStateTrumpet;

        // Set references and components
        myCC = GetComponent<CharacterController>();
        myTransform = transform;
        mainCamera = Camera.main;
        myAnimator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        // Set the initial state of the player 
        currentState = playerStateGuitar;

        // Begin cycling between states
        //InvokeRepeating("CycleStates", switchStateSeconds, switchStateSeconds);
        StartCoroutine(CycleStates());

        // Set a layer mask so that we only hit things marked as default
        layerMask = LayerMask.GetMask("Default");
    }

    IEnumerator CycleStates()
    {
        // Wait for a defined amount of time
        yield return new WaitForSeconds(switchStateSeconds - sfxSwitchWarning.length);

        // Play a sound effect
        //audioSource.PlayOneShot(sfxSwitchWarning);

        yield return new WaitForSeconds(sfxSwitchWarning.length);

        // Add to the current state index. If it is larger than the array, set it to zero.
        // Then switch states based on the index. This will allow us to loop through all of the 
        // player's states
        currentStateIndex++;
        if (currentStateIndex > allStates.Length - 1)
            currentStateIndex = 0;
        SetState(allStates[currentStateIndex]);

        // Play the sound for actually swapping weapons
        //audioSource.PlayOneShot(sfxOnSwitch);

        // Repeat the corroutine
        StartCoroutine(CycleStates());
    }

    public void SetState(PlayerStateBase newState)
    {
        // Set the last state to equal current state so that we have reference to it
        lastState = currentState;

        // Set the new state
        currentState = newState;

        // Call the exit method of the last state and the enter method of the current state
        lastState.ExitState(this);
        currentState.EnterState(this);

        // Fire the changed state event
        PlayerStateChanged();
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        // Preform whatever actions we need to do in Update based on our current state
        currentState.Update(this);

        //Attack();

        if (Input.GetKeyDown(KeyCode.F1))
        {
            SetState(playerStateGuitar);
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            SetState(playerStateRecord);
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            SetState(playerStateTrumpet);
        }
    }

    public void DisableHitbox()
    {
        guitarHitBox.enabled = false;
    }

    public void Move()
    {
        groundedPlayer = myCC.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            //myAnimator.SetTrigger("isGrounded");
            playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        myCC.Move(move * Time.deltaTime * speed);

        // Update animations

         move.Normalize();

        move = transform.InverseTransformDirection(move);

        myAnimator.SetFloat("speedX", move.x);
        myAnimator.SetFloat("speedZ", move.z);

        // ------ Old Code --------

        /*
        float speedX = move.normalized.x;
        float speedZ = move.normalized.z;

        myAnimator.SetFloat("speedX", speedX);
        myAnimator.SetFloat("speedZ", speedZ);

        */

        // ----- Disabled jumping
        /*
        // Changes the height position of the player..
        if (Input.GetKeyDown(KeyCode.Space) && groundedPlayer)
        {
            //audioSource.PlayOneShot(sfxJump);
            playerVelocity.y = Mathf.Sqrt(jumpForce * -3.0f * gravity);
            //myAnimator.SetTrigger("isJumping");
        }
        */
        if (playerVelocity.y > 0)
            playerVelocity.y += gravity * Time.deltaTime;
        else
            playerVelocity.y += gravityWhileFalling * Time.deltaTime;
        myCC.Move(playerVelocity * Time.deltaTime);


        RaycastHit mousePoint;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out mousePoint, Mathf.Infinity, layerMask))
        {
            Vector3 target = mousePoint.point;
            Vector3 direction = target - myTransform.position;
            direction.Normalize();

            myTransform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        }
    }
}
