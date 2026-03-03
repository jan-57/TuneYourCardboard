///
/// TMP CODE
///
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Ground")]
    [SerializeField] private float accel = 200f;         // How fast the player accelerates on the ground
    [SerializeField] private float maxSpeed = 6.4f;      // Maximum player speed on the ground
    [SerializeField] private float friction = 6f;        // How fast the player decelerates on the ground
    [SerializeField] private float jumpForce = 5f;       // How high the player jumps
    [Header("Air")]
    [SerializeField] private float airAccel = 200f;      // How fast the player accelerates in the air
    [SerializeField] private float maxAirSpeed = 0.6f;   // "Maximum" player speed in the air
    [Header("References")]
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private Collider playerCollider;
    [SerializeField] private GameObject camObj;
    [Space]
    [SerializeField] private float playerHalfSize = 1;
    [SerializeField] private float crouchSize;


    private float playerStandSize;

    private float lastJumpPress = -1f;
    private float jumpPressDuration = 0.1f;
    private float curAccel, curMaxSpeed;
    private bool onGround = false;

    private InputSystem_Player inputSystem;
    
    private void Awake()
    {
        playerStandSize = transform.localScale.y;
        inputSystem = new InputSystem_Player();        
    }
    private void OnEnable()
    {
        inputSystem.Enable();
    }
    private void OnDisable()
    {
        inputSystem.Disable();
    }

    private void Update()
    {
        //print(new Vector3(GetComponent<Rigidbody>().velocity.x, 0f, GetComponent<Rigidbody>().velocity.z).magnitude); Debuging purpose
        if (inputSystem.Player.Jump.IsPressed() && !PauseManager.GameIsPaused && !PauseManager.InputIsPaused)
        {
            lastJumpPress = Time.time;
        }
    }

    private void FixedUpdate()
    {
        if (!PauseManager.GameIsPaused)
        {
            //input
            Vector2 input = inputSystem.Player.Move.ReadValue<Vector2>();
            bool crouchInput = inputSystem.Player.Crouch.ReadValue<bool>();
            
            onGround = CheckGround();

            // Get player velocity
            Vector3 playerVelocity = GetComponent<Rigidbody>().linearVelocity;
            // Slow down if on ground
            playerVelocity = CalculateFriction(playerVelocity);
            // Add player input
            playerVelocity += CalculateMovement(input, playerVelocity);
            // Assign new velocity to player object
            GetComponent<Rigidbody>().linearVelocity = playerVelocity;
            // Crouch
            if (crouchInput)
            {
                transform.localScale.Set(transform.localScale.x, crouchSize, transform.localScale.z);
            }
            else
            {
                transform.localScale.Set(transform.localScale.x, playerStandSize, transform.localScale.z);
            }
        }
    }

    private Vector3 CalculateFriction(Vector3 currentVelocity)
    {
        Debug.Log("Calculate Friction");
        float speed = currentVelocity.magnitude;

        if (!onGround || inputSystem.Player.Jump.IsPressed() || speed == 0f)
            return currentVelocity;

        float drop = speed * friction * Time.deltaTime;
        return currentVelocity * (Mathf.Max(speed - drop, 0f) / speed);
    }

    private Vector3 CalculateMovement(Vector2 input, Vector3 velocity)
    {
        Debug.Log("Calculate Movement");

        //Different acceleration values for ground and air
        if (onGround) 
        { 
            curAccel = accel;
            curMaxSpeed = maxSpeed;
        }
        else
        {
            curAccel = airAccel;        
            curMaxSpeed = maxAirSpeed;
        }   

        //Get rotation input and make it a vector
        Vector3 camRotation = new Vector3(0f, camObj.transform.rotation.eulerAngles.y, 0f);
        Vector3 inputVelocity = Quaternion.Euler(camRotation) *
                                new Vector3(input.x * curAccel, 0f, input.y * curAccel);

        //Ignore vertical component of rotated input
        Vector3 alignedInputVelocity = new Vector3(inputVelocity.x, 0f, inputVelocity.z) * Time.deltaTime;

        //Get current velocity
        Vector3 currentVelocity = new Vector3(velocity.x, 0f, velocity.z);

        //How close the current speed to max velocity is (1 = not moving, 0 = at/over max speed)
        float max = Mathf.Max(0f, 1 - (currentVelocity.magnitude / curMaxSpeed));

        //How perpendicular the input to the current velocity is (0 = 90��)
        float velocityDot = Vector3.Dot(currentVelocity, alignedInputVelocity);

        //Scale the input to the max speed
        Vector3 modifiedVelocity = alignedInputVelocity * max;

        //The more perpendicular the input is, the more the input velocity will be applied
        Vector3 correctVelocity = Vector3.Lerp(alignedInputVelocity, modifiedVelocity, velocityDot);

        //Apply jump
        correctVelocity += GetJumpVelocity(velocity.y);

        //Return
        return correctVelocity;
    }

    private Vector3 GetJumpVelocity(float yVelocity)
    {
        Vector3 jumpVelocity = Vector3.zero;

        if (Time.time < lastJumpPress + jumpPressDuration && yVelocity < jumpForce && CheckGround())
        {
            lastJumpPress = -1f;
            jumpVelocity = new Vector3(0f, jumpForce - yVelocity, 0f);
        }

        return jumpVelocity;
    }

    public bool CheckGround() //could be improved
    {
        Vector3 bottom = playerCollider.bounds.center - new Vector3(0, playerCollider.bounds.extents.y, 0);
        float radius = playerCollider.bounds.extents.x * 0.9f;
        return Physics.CheckSphere(bottom + Vector3.up * 0.1f, radius, groundLayers);
    }
}