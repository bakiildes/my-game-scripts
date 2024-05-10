using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float Speed;
    public float crouchSpeed = 1f; // Slower speed for crouching
    public float normalSpeed = 6f;
    public float sprintSpeed = 12f;
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaDrainRate = 20f; // Stamina drains per second
    public float staminaRecoveryRate = 10f; // Stamina recovers per second
    public float gravity = -9.81f;
    public float groundDistance = 0.4f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;
    private bool isCrouching = false;
    private bool isSprinting = false; // Add flag for sprinting state

    // Start is called before the first frame update
    void Start()
    {
        Speed = normalSpeed;
        currentStamina = maxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -4f;
        }

        float x = UnityEngine.Input.GetAxisRaw("Horizontal");
        float z = UnityEngine.Input.GetAxisRaw("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;

        float targetSpeed = isCrouching ? crouchSpeed : (isSprinting ? sprintSpeed : normalSpeed); // Determine speed
        move *= targetSpeed * Time.deltaTime;

        controller.Move(move);

        if (UnityEngine.Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime * 2;
        controller.Move(velocity * Time.deltaTime);

        if (UnityEngine.Input.GetButtonDown("Crouch"))
        {
            StartCrouching();
        }

        if (UnityEngine.Input.GetButtonUp("Crouch"))
        {
            StopCrouching();
        }

        if (Input.GetButtonDown("Sprint") && currentStamina > 0 && currentStamina >= 30)
        {
            Speed = sprintSpeed;
            isSprinting = true;
            

        }

        if (isSprinting && currentStamina > 0 && (x != 0 || z != 0))   // Continuously drain stamina while sprinting
        {
            currentStamina -= staminaDrainRate * Time.deltaTime;
        }

        if (Input.GetButtonUp("Sprint") || currentStamina <= 0) // Stop if sprint is released or stamina is depleted
        {
            Speed = normalSpeed;
            isSprinting = false;
        }

        // Manage stamina outside of sprinting
        if (currentStamina < maxStamina && !isSprinting) // Only recover when not sprinting
        {
            currentStamina += staminaRecoveryRate * Time.deltaTime;
        }
    }

    public void StartCrouching()
    {
        transform.localScale = new Vector3(x: 1f, y: 0.5f, z: 1);
        isCrouching = true;
    }

    public void StopCrouching()
    {
        transform.localScale = new Vector3(x: 1f, y: 1, z: 1f);
        isCrouching = false;
    }

   
}
