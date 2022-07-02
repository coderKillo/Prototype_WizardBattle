using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class FirstPersonController : MonoBehaviour
{
    [Header("References")]
    public Transform head;
    public Transform orientation;
    public Transform body;
    private Rigidbody rb;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 4500;
    [SerializeField] private float maxSpeed = 20;
    [SerializeField] private bool grounded;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField][Range(0f, 0.5f)] private float counterMovement = 0.175f;
    [SerializeField][Range(0f, 0.1f)] private float threshold = 0.01f;
    [SerializeField][Range(0f, 90f)] private float maxSlopeAngle = 35f;
    private bool cancellingGrounded;

    [Header("Rotation and Rook")]
    [SerializeField] private float sensitivity = 50f;
    [SerializeField][Range(0f, 1f)] private float sensMultiplier = 1f;
    private float xRotation;
    private float yRotation;

    [Header("Crouch & Slide")]
    [SerializeField] private float slideForce = 400;
    [SerializeField][Range(0f, 1f)] private float slideCounterMovement = 0.2f;
    private Vector3 crouchScale;
    private Vector3 playerScale;

    [Header("Jumping")]
    [SerializeField] private float jumpForce = 550f;
    private bool readyToJump = true;
    private float jumpCooldown = 0.25f;
    private Vector3 normalVector = Vector3.up;

    // Inputs
    private Vector2 inputMove;
    private Vector2 inputLook;
    private bool inputJumping;
    private bool inputSprinting;
    private bool inputCrouching;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        playerScale = body.transform.localScale;
        crouchScale = playerScale;
        crouchScale.y *= 0.5f;

        // Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    ////////////////////////////////////////////////////////////////////////////////

    private void FixedUpdate()
    {
        ExtraGravity();
        Movement();
    }

    private void Update()
    {
        Look();
    }

    ////////////////////////////////////////////////////////////////////////////////

    private void OnMove(InputValue value)
    {
        inputMove = value.Get<Vector2>();
    }

    private void OnLook(InputValue value)
    {
        inputLook = value.Get<Vector2>();
    }

    private void OnJump(InputValue value)
    {
        inputJumping = value.isPressed;

        if (inputJumping)
        {
            Jump();
        }
    }

    private void OnCrouch(InputValue value)
    {
        inputCrouching = value.isPressed;

        if (inputCrouching)
        {
            StartCrouch();
        }
        else
        {
            StopCrouch();
        }
    }

    private void OnSprint(InputValue value)
    {
        inputSprinting = value.isPressed;
    }


    ////////////////////////////////////////////////////////////////////////////////

    private void Movement()
    {
        var movementVec = inputMove;
        var maxSpeed = this.maxSpeed;
        Vector2 mag = FindVelRelativeToLook();

        CounterSlidingAndSloppyMovement(movementVec.x, movementVec.y, mag);

        //If sliding down a ramp, add force down so player stays grounded and also builds speed
        if (inputCrouching && grounded && readyToJump)
        {
            rb.AddForce(Vector3.down * Time.deltaTime * 3000);
            return;
        }

        movementVec = ClampInputOnMaxMovementSpeed(movementVec, maxSpeed, mag);
        movementVec *= GetMovementMultiplier();

        rb.AddForce(orientation.transform.forward * movementVec.y * moveSpeed * Time.deltaTime);
        rb.AddForce(orientation.transform.right * movementVec.x * moveSpeed * Time.deltaTime);
    }

    private static Vector2 ClampInputOnMaxMovementSpeed(Vector2 movementVec, float maxSpeed, Vector2 mag)
    {
        if (movementVec.x > 0 && mag.x > maxSpeed) movementVec.x = 0;
        if (movementVec.x < 0 && mag.x < -maxSpeed) movementVec.x = 0;
        if (movementVec.y > 0 && mag.y > maxSpeed) movementVec.y = 0;
        if (movementVec.y < 0 && mag.y < -maxSpeed) movementVec.y = 0;
        return movementVec;
    }

    private Vector2 GetMovementMultiplier()
    {
        Vector2 movementMultiplier = new Vector2(1f, 1f);

        // Movement in air
        if (!grounded)
        {
            movementMultiplier.x = 0.5f;
            movementMultiplier.y = 0.5f;
        }

        // Movement while sliding
        if (grounded && inputCrouching)
        {
            movementMultiplier.y = 0f;
        }

        return movementMultiplier;
    }

    private void ExtraGravity()
    {
        rb.AddForce(Vector3.down * Time.deltaTime * 10);
    }

    private void Look()
    {
        float mouseX = inputLook.x * sensitivity * Time.deltaTime * sensMultiplier;
        float mouseY = inputLook.y * sensitivity * Time.deltaTime * sensMultiplier;

        Vector3 playerCamRotation = head.transform.localRotation.eulerAngles;
        yRotation = playerCamRotation.y + mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        head.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.transform.localRotation = Quaternion.Euler(0, yRotation, 0);
    }

    private void CounterSlidingAndSloppyMovement(float x, float y, Vector2 mag)
    {
        if (!grounded || inputJumping) { return; }

        //Slow down sliding
        if (inputCrouching)
        {
            rb.AddForce(moveSpeed * Time.deltaTime * -rb.velocity.normalized * slideCounterMovement);
            return;
        }

        //Counter movement
        if (Mathf.Abs(mag.x) > threshold && Mathf.Abs(x) < 0.05f || (mag.x < -threshold && x > 0) || (mag.x > threshold && x < 0))
        {
            rb.AddForce(moveSpeed * orientation.transform.right * Time.deltaTime * -mag.x * counterMovement);
        }
        if (Mathf.Abs(mag.y) > threshold && Mathf.Abs(y) < 0.05f || (mag.y < -threshold && y > 0) || (mag.y > threshold && y < 0))
        {
            rb.AddForce(moveSpeed * orientation.transform.forward * Time.deltaTime * -mag.y * counterMovement);
        }

        //Limit diagonal running. This will also cause a full stop if sliding fast and un-crouching, so not optimal.
        if (Mathf.Sqrt((Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2))) > maxSpeed)
        {
            float fallspeed = rb.velocity.y;
            Vector3 n = rb.velocity.normalized * maxSpeed;
            rb.velocity = new Vector3(n.x, fallspeed, n.z);
        }
    }

    public Vector3 FindVelRelativeToLook()
    {
        float lookAngle = orientation.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitue = rb.velocity.magnitude;
        float yMag = magnitue * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitue * Mathf.Cos(v * Mathf.Deg2Rad);

        return new Vector2(xMag, yMag);
    }

    ////////////////////////////////////////////////////////////////////////////////
    // Handle ground detection
    ////////////////////////////////////////////////////////////////////////////////

    private void OnCollisionStay(Collision other)
    {
        //Make sure we are only checking for walkable layers
        int layer = other.gameObject.layer;
        if (whatIsGround != (whatIsGround | (1 << layer))) return;

        //Iterate through every collision in a physics update
        for (int i = 0; i < other.contactCount; i++)
        {
            Vector3 normal = other.contacts[i].normal;
            //FLOOR
            if (IsFloor(normal))
            {
                grounded = true;
                cancellingGrounded = false;
                normalVector = normal;
                CancelInvoke(nameof(StopGrounded));
            }
        }

        //Invoke ground/wall cancel, since we can't check normals with CollisionExit
        float delay = 3f;
        if (!cancellingGrounded)
        {
            cancellingGrounded = true;
            Invoke(nameof(StopGrounded), Time.deltaTime * delay);
        }
    }

    private bool IsFloor(Vector3 v)
    {
        float angle = Vector3.Angle(Vector3.up, v);
        return angle < maxSlopeAngle;
    }


    private void StopGrounded()
    {
        grounded = false;
    }

    ////////////////////////////////////////////////////////////////////////////////

    private void StartCrouch()
    {
        body.transform.localScale = crouchScale;
        if (rb.velocity.magnitude > 0.5f)
        {
            if (grounded)
            {
                rb.AddForce(orientation.transform.forward * slideForce);
            }
        }
    }

    private void StopCrouch()
    {
        body.transform.localScale = playerScale;
    }

    ////////////////////////////////////////////////////////////////////////////////

    private void Jump()
    {
        if (grounded && readyToJump)
        {
            readyToJump = false;

            //Add jump forces
            rb.AddForce(Vector2.up * jumpForce * 1.5f);
            rb.AddForce(normalVector * jumpForce * 0.5f);

            //If jumping while falling, reset y velocity.
            Vector3 vel = rb.velocity;
            if (rb.velocity.y < 0.5f)
                rb.velocity = new Vector3(vel.x, 0, vel.z);
            else if (rb.velocity.y > 0)
                rb.velocity = new Vector3(vel.x, vel.y / 2, vel.z);

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    ////////////////////////////////////////////////////////////////////////////////

}
