using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    public Camera playerCamera;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 7f;
    public float gravity = 10f;

    public float lookSpeed = 2f;
    public float lookXLimit = 45f;

    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    public bool canMove = true;

    CharacterController characterController;

    [Header("FMOD Audio")]
    public EventReference footstepEvent;
    public float walkStepInterval = 0.5f;
    public float runStepInterval = 0.35f;
    private float stepTimer = 99f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        #region Handles Movement
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        // Press Left Shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);
        #endregion

        #region Handles Jumping
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
        #endregion

        #region Handles Rotation
        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
        #endregion

        UpdateFootsteps(isRunning);
    }

    // Logika odtwarzania kroków
    void UpdateFootsteps(bool isRunning)
    {
        stepTimer += Time.deltaTime;

        // Sprawdzenie czy gracz stoi na ziemi
        if (!characterController.isGrounded) return;

        // Sprawdzamy prędkość poruszania się
        Vector3 horizontalVelocity = new Vector3(characterController.velocity.x, 0, characterController.velocity.z);

        if (horizontalVelocity.magnitude > 0.1f)
        {
            // Dobieramy odpowiedni interwał czasowy (bieg vs chód)
            float currentInterval = isRunning ? runStepInterval : walkStepInterval;

            // Dźwięk zagra TYLKO wtedy, gdy od poprzedniego kroku minęło odpowiednio dużo czasu
            if (stepTimer >= currentInterval)
            {
                PlayFootstepSound();
                stepTimer = 0f;
            }
        }
    }

    void PlayFootstepSound()
    {
        FMOD.Studio.EventInstance footstepInstance = RuntimeManager.CreateInstance(footstepEvent);

        // Ustawiamy pozycję 3D dźwięku na pozycji gracza
        footstepInstance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));

        // Domyślny tag, jeśli raycast w nic nie trafi lub trafi w obiekt bez odpowiedniego tagu
        string surfaceTag = "Wood";

        RaycastHit hit;
        // Wystrzeliwujemy promień w dół
        float rayDistance = (characterController.height / 2f) + 0.3f;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, rayDistance))
        {
            // Sprawdzamy, czy obiekt pod nami ma jeden z interesujących nas tagów
            if (hit.collider.CompareTag("Wood") || hit.collider.CompareTag("Stone") || hit.collider.CompareTag("Stairs"))
            {
                surfaceTag = hit.collider.tag;
            }
        }

        // Przekazujemy nazwę tagu bezpośrednio jako Label do parametru "Surface" we FMOD
        footstepInstance.setParameterByNameWithLabel("Surface", surfaceTag);

        //NOWE: Uruchamiamy dźwięk i od razu dajemy FMOD-owi znak, że po zakończeniu odtwarzania ma posprzątać pamięć
        footstepInstance.start();
        footstepInstance.release();
    }
}