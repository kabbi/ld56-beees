using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5;
    public float verticalSpeed = 1;
    public float gravityFactor = 1;
    public float runSpeed = 5;
    public float jumpForce = 5;
    public float lookRotationDampFactor = 10;
    private State state = State.Moving;
    private PlayerInput playerInput;
    private CharacterController controller;
    private Transform ourCamera;
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private InputAction runAction;
    private InputAction flyAction;
    private bool runTriggered;
    private bool jumpTriggered;
    private bool bonkTriggered;
    private bool flyTriggered;
    private Vector3 bonkDirection;
    private Vector3 moveVelocity;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private float verticalInput;

    enum State
    {
        // Jumping,
        // Falling,
        Moving,
    }

    void Awake()
    {
        ourCamera = Camera.main.transform;
        playerInput = GetComponent<PlayerInput>();
        controller = GetComponent<CharacterController>();
        moveAction = playerInput.currentActionMap.FindAction("Move");
        lookAction = playerInput.currentActionMap.FindAction("Look");
        jumpAction = playerInput.currentActionMap.FindAction("Jump");
        runAction = playerInput.currentActionMap.FindAction("Sprint");
        flyAction = playerInput.currentActionMap.FindAction("Fly");
    }

    void OnEnable()
    {
        playerInput.onActionTriggered += HandleAction;
    }

    void OnDisable()
    {
        playerInput.onActionTriggered -= HandleAction;
    }

    public void SetVerticalInput(float v)
    {
        verticalInput = v;
    }

    private void HandleAction(InputAction.CallbackContext context)
    {
        if (context.action == moveAction)
        {
            moveInput = context.ReadValue<Vector2>();
        }
        if (context.action == lookAction)
        {
            lookInput = context.ReadValue<Vector2>();
        }
        if (context.action == jumpAction && context.performed)
        {
            jumpTriggered = true;
        }
        if (context.action == runAction)
        {
            runTriggered = context.ReadValue<float>() > 0;
        }
        if (context.action == flyAction)
        {
            flyTriggered = context.ReadValue<float>() > 0;
            verticalInput = context.ReadValue<float>();
        }
    }

    public void Bonk(Vector3 direction)
    {
        bonkDirection = direction;
        bonkTriggered = true;
    }

    private void CalculateMoveDirection()
    {
        Vector3 cameraForward = new(ourCamera.forward.x, 0, ourCamera.forward.z);
        Vector3 cameraRight = new(ourCamera.right.x, 0, ourCamera.right.z);

        Vector3 moveDirection = cameraForward.normalized * moveInput.y + cameraRight.normalized * moveInput.x;

        float speed = runTriggered ? runSpeed : moveSpeed;
        moveVelocity.x = moveDirection.x * speed;
        if (verticalInput != 0)
        {
            moveVelocity.y = verticalInput * verticalSpeed;
        }
        moveVelocity.z = moveDirection.z * speed;
    }

    private void FaceMoveDirection()
    {

        Vector3 faceDirection = new(moveVelocity.x, 0f, moveVelocity.z);

        if (faceDirection == Vector3.zero)
        {
            return;
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(faceDirection), lookRotationDampFactor * Time.deltaTime);
    }

    private void ApplyGravity()
    {
        if (moveVelocity.y > Physics.gravity.y)
        {
            moveVelocity.y += Physics.gravity.y * Time.deltaTime * gravityFactor;
        }
    }

    private void Move()
    {
        controller.Move(moveVelocity * Time.deltaTime);
    }

    private void ApplyBonk()
    {
        if (bonkTriggered)
        {
            moveVelocity += bonkDirection;
            bonkTriggered = false;
        }
    }

    // private void HandleJumping() {
    //     // Should we wait for velocity to become negative here?
    //     if (!controller.isGrounded) {
    //         state = State.Falling;
    //     }

    //     CalculateMoveDirection();
    //     FaceMoveDirection();
    //     ApplyGravity();
    //     Move();
    // }

    // private void HandleFalling() {
    //     if (controller.isGrounded) {
    //         // Clear any jump queried while jumping (could check in event handler above though)
    //         jumpTriggered = false;
    //         state = State.Moving;
    //     }

    //     CalculateMoveDirection();
    //     FaceMoveDirection();
    //     ApplyGravity();
    //     Move();
    // }

    private void HandleMoving()
    {
        // if (jumpTriggered) {
        //     moveVelocity.Set(moveVelocity.x, jumpForce, moveVelocity.z);
        //     state = State.Jumping;
        //     jumpTriggered = false;
        // }

        CalculateMoveDirection();
        FaceMoveDirection();
        ApplyGravity();
        ApplyBonk();
        Move();
    }

    void Update()
    {
        // if (state == State.Jumping) {
        //     HandleJumping();
        // }
        // if (state == State.Falling) {
        //     HandleFalling();
        // }
        if (state == State.Moving)
        {
            HandleMoving();
        }
    }
}
