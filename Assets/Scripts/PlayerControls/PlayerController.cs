using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;
    private Vector3 movement;
    private Vector3 movementVector;
    private bool isMoving;

    public float sprintFactor = 1.5f;
    public Slider staminaBar;
    private bool canRun;

    public GameObject cam;
    public float cameraSensitivity = 1f;
    private Vector2 lookAngles;

    private Animator cameraAnimator;

    private int health;
    public int maxHealth = 3;
    public AudioClip hitSound;

    private float damping = 5f;

    private int meter;
    public int maxStamina = 500;

    private InputActions input;

    void Awake()
    {
        input = new();

        cameraAnimator = cam.GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        health = maxHealth;
        staminaBar.maxValue = maxStamina;
        meter = maxStamina;
        staminaBar.value = meter;
    }
    
    private void Start()
    {
        UIManager.Instance.UpdateHealth(health);
    }

    private void OnEnable()
    {
        input.Enable();
    }
    private void OnDisable()
    {
        input.Disable();
    }

    void Update()
    {
        // Set whether player can run
        if (meter >= maxStamina)
        {
            meter = maxStamina;
            canRun = true;
        }
        if (meter <= 0)
        {
            meter = 0;
            canRun = false;
        }
        staminaBar.value = meter;

        Move();
        Look();
        cameraAnimator.SetBool("isWalking", isMoving);
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space) && meter != 0 && canRun && isMoving)
        {
            meter -= 2;
            Debug.Log("Am Sprinting!");
        }
        else
        {
            meter += 1;
        }
    }

    void Move()
    {
        movement = input.Player.Movement.ReadValue<Vector2>();
        if (movement.magnitude != 0)
        {
            movement = new Vector3(
                movement.x,
                0,
                movement.y
            );
            movementVector = moveSpeed * Time.deltaTime * movement.normalized;
            isMoving = true;
        }
        else
        {
            movementVector = Vector3.Lerp(movementVector, Vector3.zero, damping * Time.deltaTime);
            isMoving = false;
        }

        if (Input.GetKey(KeyCode.Space) && meter != 0 && canRun && isMoving)
        {
            transform.Translate(sprintFactor * movementVector);
            if (0 < Physics.OverlapSphere(transform.position, 1.5f, GameManager.active.obstacleLayers).Length)
                transform.Translate(-sprintFactor * movementVector);
        }
        else
        {
            transform.Translate(movementVector);
            if (0 < Physics.OverlapSphere(transform.position, 1.5f, GameManager.active.obstacleLayers).Length)
                transform.Translate(-movementVector);
        }
    }

    private void Look()
    {
        lookAngles += input.Player.MouseDelta.ReadValue<Vector2>() * cameraSensitivity;
        transform.rotation = Quaternion.Euler(0, lookAngles.x, 0);
    }

    public void DamagePlayer()
    {
        health--;
        UIManager.Instance.UpdateHealth(health);
        if (health < 0)
        {
            GameManager.active.OnSleep();
        }

        GameManager.active.universalSoundEffect.PlayOneShot(hitSound);
    }
}
