using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
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

    #region Audio
    [Header("Audio Clips")]
    [SerializeField]
    AudioClip sfxJump;

    #endregion

    #region Components and References

    [Header("References")]
    public GameObject armSocket;

    Camera mainCamera;

    CharacterController myCC;                       // The character controller on this object
    //Rigidbody myRigidbody;
    Transform myTransform;
    Animator myAnimator;
    AudioSource audioSource;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // Set references and components
        myCC = GetComponent<CharacterController>();
        myTransform = transform;
        mainCamera = Camera.main;
        myAnimator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        layerMask = LayerMask.GetMask("Default");

    }

    // Update is called once per frame
    void Update()
    {
        Move();
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
        float speedX = move.normalized.x;
        float speedZ = move.normalized.z;

        //myAnimator.SetFloat("SpeedX", speedX);
        //myAnimator.SetFloat("SpeedZ", speedZ);

        // Changes the height position of the player..
        if (Input.GetKeyDown(KeyCode.Space) && groundedPlayer)
        {
            //audioSource.PlayOneShot(sfxJump);
            playerVelocity.y = Mathf.Sqrt(jumpForce * -3.0f * gravity);
            //myAnimator.SetTrigger("isJumping");
        }

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

    /*
    public void Throw()
    {

        //Make sure arm socket is assigned
        if(armSocket == null)
        {
            Debug.LogError("Arm socket not assigned");
            return;
        }

        // Cancel function if socket does not have a child
        Holdable heldObject = armSocket.GetComponentInChildren<Holdable>();
        if (heldObject == null)
            return;

        heldObject.Detatch(throwStrength, torque, lift);

    }
    */
}
