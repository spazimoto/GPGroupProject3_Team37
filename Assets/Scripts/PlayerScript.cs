using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 1f;
    [SerializeField] private Transform debugHitPointTransform;
    [SerializeField] private Transform hookshotTransform;

    [SerializeField] private float speed, gravity;

    public float knockbackForce, knockbackTime;
    private float knockbackCounter;

    private CameraController cameraFOV;
    private float cameraVertAngle;
    private float cameraHorAngle;

    private const float normalFOV = 60f;
    private const float hookshotFOV = 100f;

    private float playerVelocityY;
    private Vector3 playerVelocityMomentum;

    public Transform camera;
    public CharacterController controller;
    public CameraShake camShake;

    private State state;
    private Vector3 hookshotPosition;
    private float hookshotSize;

    public bool lightMass, normalMass, heavyMass;

    private bool inWindArea;

    [SerializeField] private float cooldownTime = 20f;
    private float nextMassChange = 0f;

    public Animator anim;

    Animation animation;

    public GameObject turbineWind;
    public Transform turbine;

    AudioSource SFX;
    public AudioClip jumping;
    public AudioClip shootHook;

    public GameObject crosshair;

    public static bool gotWheel = false;
    public static bool gotEngine = false;
    public static bool gotSail = false;

    public GameObject wheel;
    public GameObject sail;
    public GameObject engine;
    
    // this is where my public static gameobjects would go...IF I HAD ANY!!

    private enum State
    {
        Normal, 
        HookshotThrown,
        HookshotPull
    }

    private void Awake()
    {
        state = State.Normal;

        cameraFOV = camera.GetComponent<CameraController>();

        SFX = gameObject.GetComponent<AudioSource>();

        hookshotTransform.gameObject.SetActive(false);
    }

    private void Start()
    {
        speed = 50f;
        gravity = -100f;

        lightMass = false;
        normalMass = true;
        heavyMass = false;

        anim.SetTrigger("isAlive");

        ObjectiveClear();
    }

    void Update()
    {
        if (gotWheel && gotEngine && gotSail)
        {
            SceneManager.LoadScene("Win");
        }

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
            case State.HookshotPull:
                HookshotMovement();
                MouseLook();
                break;
        }

        ChangeMass();
        AnimationControl();
    }

    void MouseLook()
    {
        float lookX = Input.GetAxisRaw("Mouse X");
        float lookY = Input.GetAxisRaw("Mouse Y");

        if(!PauseMenu.gamePaused)
        {
            transform.Rotate(new Vector3(0f, lookX * mouseSensitivity, 0f), Space.Self);

            cameraVertAngle -= lookY * mouseSensitivity;
            cameraHorAngle -= lookX * mouseSensitivity;

            cameraVertAngle = Mathf.Clamp(cameraVertAngle, -89f, 89f);

            camera.transform.localEulerAngles = new Vector3(cameraVertAngle, 0, 0);
        }
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
                float jumpSpeed = 45f;
                playerVelocityY = jumpSpeed;
                SFXControl(jumping);
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

    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.CompareTag("Damage"))
        {
            print("Yowch!");
            Knockback();
        }

        if(collider.gameObject.CompareTag("Wheel"))
        {
            Destroy(collider.gameObject);
            gotWheel = true;

            SceneManager.LoadScene("Ezra's Market (Hub Level)");
        }
        
        if(collider.gameObject.CompareTag("Sail"))
        {
            Destroy(collider.gameObject);
            gotSail = true;

            SceneManager.LoadScene("Ezra's Market (Hub Level)");
        }

        if(collider.gameObject.CompareTag("Engine"))
        {
            Destroy(collider.gameObject);
            gotEngine = true;

            SceneManager.LoadScene("Ezra's Market (Hub Level)");
        }

    }

    void OnTriggerStay(Collider collider)
    {
        if(collider.gameObject.CompareTag("Wind"))
        {
            WindArea windArea = collider.gameObject.GetComponent<WindArea>();
            inWindArea = true;

            if(!heavyMass)
            {
                print("Ahhh! I'm being blown away!");
                controller.Move(windArea.direction.normalized * windArea.strength * Time.deltaTime);
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        //print ("Phew! That was close!");
        inWindArea = false;
    }

    void ChangeMass()
    {
        if (Time.time > nextMassChange)
        {
            //print ("Your mass is now set to normal.");
            normalMass = true;

            lightMass = false;
            heavyMass = false;

            speed = 20f;
            gravity = -100f;

            if(Input.GetKeyDown(KeyCode.Q) || Input.GetButtonDown("Light"))
            {
                print("Your mass is now set to light.");
                lightMass = true;

                normalMass = false;
                heavyMass = false;

                speed = 30f;
                gravity = -60f;

                Cooldown();
            }

            if(Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("Heavy"))
            {
                print("Your mass is now set to heavy.");
                heavyMass = true;

                lightMass = false;
                normalMass = false;

                speed = 10f; 
                gravity = -120f;
                Cooldown();
            }
        }
    }

    void ResetGravity()
    {
        playerVelocityY = 0f;
    }

    void HandleHookshotStart()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 100)) //origin, direction, returns a boolean if hit = true
        {
            if(raycastHit.collider.tag == "Grapple")
            {
                crosshair.GetComponent<Image>().color = Color.red;
                if (HookshotInput() || Input.GetAxis("Firehook") != 0)
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
        else
        {
            crosshair.GetComponent<Image>().color = Color.white;
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
            SFXControl(shootHook);
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
        if (HookshotInput() || Input.GetAxis("Firehook") != 0)
        {
            CancelHookshot();
        }

        if (JumpInput())
        {
            float momentumExtraSpeed = 7f;
            playerVelocityMomentum = hookshotDir * hookshotSpeed * momentumExtraSpeed;

            float jumpSpeed = 40f;
            playerVelocityMomentum += Vector3.up * jumpSpeed;

            SFXControl(jumping);
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
        return Input.GetButton("Jump");
    }

    void Cooldown()
    {
        nextMassChange = Time.time + cooldownTime;
    }

    public void Knockback()
    {
        knockbackCounter = knockbackTime;
        Vector3 impactDirection = new Vector3(2f,1f,2f);

        controller.Move(impactDirection * knockbackForce);
        //transform.Translate(Vector3.back, Space.Self);
    }

    void AnimationControl()
    {
        if(!PauseMenu.gamePaused)
        {
            if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                anim.SetBool("isWalking", true);
                anim.SetBool("isAlive", false);
            }
            else
            {
                anim.SetBool("isWalking", false);
            }

            if (controller.isGrounded)
            {
                anim.SetBool("isJumping", false);
                print("I'm walking...on the GROUND!");
            }
            else
            {
                anim.SetBool("isJumping", true);
                anim.SetBool("isWalking", false);
                print("Whoa! Where's the ground?!");
            }
        }
    }

    public void SFXControl(AudioClip audio)
    {
        SFX.PlayOneShot(audio);
    }

    void WinPlaceholder()
    {
        SceneManager.LoadScene("WinPlaceholder");
    }

    void ObjectiveClear()
    {
        if(gotWheel)
        {
            Destroy(wheel);
        }
        if(gotEngine)
        {
            Destroy(engine);
        }
        if(gotSail)
        {
            Destroy(sail);
        }
    }
}
