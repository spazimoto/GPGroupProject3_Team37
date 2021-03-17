using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 1f;
    [SerializeField] private Transform debugHitPointTransform;
    [SerializeField] private Transform hookshotTransform;

    [SerializeField] private float speed;
    [SerializeField] private float gravity;

    private CameraController cameraFOV;
    private float cameraVertAngle;
    private float cameraHorAngle;

    private const float normalFOV = 60f;
    private const float hookshotFOV = 100f;

    private float playerVelocityY;
    private Vector3 playerVelocityMomentum;

    public Transform head;

    public Transform camera;
    public CharacterController controller;
    public CameraShake camShake;

    private State state;
    private Vector3 hookshotPosition;
    private float hookshotSize;

    private bool heavyMass;

    private enum State
    {
        Normal, 
        HookshotThrown,
        HookshotPull
    }

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        state = State.Normal;

        cameraFOV = camera.GetComponent<CameraController>();

        hookshotTransform.gameObject.SetActive(false);
    }

    private void Start()
    {
        speed = 20f;
        gravity = -60f;

        heavyMass = false;
    }

    void Update()
    {
        switch(state)
        {
            default:
            case State.Normal:
                MouseLook();
                PlayerMovement();
                HandleHookshotStart();
                break;
            case State.HookshotThrown:
                HookshotLaunch();
                MouseLook();
                PlayerMovement();
                break;
            case State.HookshotPull: // good as is, don't need to adjust it at all? if the player is too heavy, have the ui throw an error saying that it can't support weight
                HookshotMovement();
                MouseLook();
                break;
        }

        ChangeMass();
    }

    void MouseLook()
    {
        float lookX = Input.GetAxisRaw("Mouse X");
        float lookY = Input.GetAxisRaw("Mouse Y");

        transform.Rotate(new Vector3(0f, lookX * mouseSensitivity, 0f), Space.Self);

        head.transform.Rotate(new Vector3(lookY * mouseSensitivity, 0f, 0f));

        cameraVertAngle -= lookY * mouseSensitivity;
        cameraHorAngle -= lookX * mouseSensitivity;

        cameraVertAngle = Mathf.Clamp(cameraVertAngle, -89f, 89f);

        camera.transform.localEulerAngles = new Vector3(cameraVertAngle, 0, 0);
    }

    void PlayerMovement()
    {
        float hMovement = Input.GetAxisRaw("Horizontal");
        float zMovement = Input.GetAxisRaw("Vertical");

        Vector3 playerVelocity = transform.right * hMovement * speed + transform.forward * zMovement * speed;

        if (controller.isGrounded)
        {
            playerVelocityY = 0f;

            if (JumpInput())
            {
                float jumpSpeed = 30f;
                playerVelocityY = jumpSpeed;

                /*if (heavyMass = true && controller.isGrounded)
                {
                    StartCoroutine(camShake.Shake(.10f, .2f));
                }*/ // heavy player camera shake, still needs polishing. currently if player switches to other modes after this camerashake is still enabled. add bool?
            }
        }

        playerVelocityY += gravity * Time.deltaTime; //fix playervelocityy to fit with mass changes

        playerVelocity.y = playerVelocityY;

        //current momentum calculated first
        playerVelocity += playerVelocityMomentum;

        controller.Move(playerVelocity * Time.deltaTime);

         //dampens momentum
        if (playerVelocityMomentum.magnitude >= 0f)
        {
            float momentumDrag = 3f;
            playerVelocityMomentum -= playerVelocityMomentum * momentumDrag * Time.deltaTime;

            if (playerVelocityMomentum.magnitude < .0f)
            {
                playerVelocityMomentum = Vector3.zero;
            }
        }
    }

    void ChangeMass()
    {
        if(Input.GetKeyDown(KeyCode.Keypad1))
        {
            print("Your mass is now set to light.");
            speed = 40f;
            gravity = -30f;
        }

        if(Input.GetKeyDown(KeyCode.Keypad2))
        {
            print("Your mass is now set to normal.");
            speed = 20f;
            gravity = -60f;
        }

        if(Input.GetKeyDown(KeyCode.Keypad3))
        {
            bool heavyMass = true;
            print("Your mass is now set to heavy.");
            speed = 5f;
            gravity = -90f;
        }
    }

    void ResetGravity()
    {
        playerVelocityY = 0f;
    }

    void HandleHookshotStart()
    {
        if (HookshotInput())
        {
            if (Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit raycastHit)) //origin, direction, returns a boolean if hit = true
            {
                debugHitPointTransform.position = raycastHit.point;
                hookshotPosition = raycastHit.point;
                hookshotSize = 0f;
                hookshotTransform.gameObject.SetActive(true);
                hookshotTransform.localScale = Vector3.zero;
                state = State.HookshotThrown;
            }
        }
    }

    void HookshotLaunch()
    {
        hookshotTransform.LookAt(hookshotPosition);

        float hookshotThrowSpeed = 200f;
        hookshotSize += hookshotThrowSpeed * Time.deltaTime;
        hookshotTransform.localScale = new Vector3(1, 1, hookshotSize);

        if (hookshotSize >= Vector3.Distance(transform.position, hookshotPosition))
        {
            state = State.HookshotPull;
            cameraFOV.SetCameraFOV(hookshotFOV);
        }
    }

    void HookshotMovement()
    {
        hookshotTransform.LookAt(hookshotPosition);

        Vector3 hookshotDir = (hookshotPosition - transform.position).normalized;

        float hookshotSpeedMin = 10f;
        float hookshotSpeedMax = 50f;
        float hookshotSpeed = Mathf.Clamp(Vector3.Distance(transform.position, hookshotPosition), hookshotSpeedMin, hookshotSpeedMax);
        float speedMultiplier = 2f;

        controller.Move(hookshotDir * hookshotSpeed * speedMultiplier * Time.deltaTime);

        float destinationReached = 2f;

        if (Vector3.Distance(transform.position, hookshotPosition) < destinationReached)
        {
            CancelHookshot();
        }

        //Cancel hookshot
        if (HookshotInput())
        {
            CancelHookshot();
        }

        if (JumpInput())
        {
            float momentumExtraSpeed = 7f;
            playerVelocityMomentum = hookshotDir * hookshotSpeed * momentumExtraSpeed;

            float jumpSpeed = 40f;
            playerVelocityMomentum += Vector3.up * jumpSpeed;
            CancelHookshot();
        }
    }

    private void CancelHookshot()
    {
        state = State.Normal;
        ResetGravity();
        hookshotTransform.gameObject.SetActive(false);
        cameraFOV.SetCameraFOV(normalFOV);
    }

    private bool HookshotInput()
    {
        return Input.GetMouseButtonDown(0);
    }

    private bool JumpInput()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }
}
