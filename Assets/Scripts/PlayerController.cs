using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    public Camera cam;
    public float moveForce = 10000;
    float maxSPeed = 10;
    public float jumpForce = 3000f; // Force applied for jumping
    public float mouseSensitivity = 100;
    private float xRotation = 0f; // Keep track of the current X rotation
    private bool cursorLocked = true; // Track whether the cursor is locked
    private bool isGrounded = true; // Track if the player is on the ground
    private bool isPlaying = true;
    public GameObject manMesh;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cursorLocked = true;
    }

    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        cursorLocked = false;
    }

    void OnCollisionStay(Collision collision)
    {
        // Check if the object the player is colliding with is the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // Player is not on the ground anymore
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Player is not on the ground anymore
        if (collision.gameObject.CompareTag("Car"))
        {
            isPlaying = false;
            manMesh.SetActive(true);
            GetComponent<MeshFilter>().mesh = null;
            rb.constraints = RigidbodyConstraints.None;
            cam.transform.position = cam.transform.position - cam.transform.forward * 5;
            cam.transform.parent = transform.parent;
        }
    }


    void Update()
    {

        if (!isPlaying)
        {
            cam.transform.position = transform.position - cam.transform.forward * 5;
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            LockCursor();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnlockCursor();
        }

        if (Input.GetKey(KeyCode.W))
            rb.AddForce(transform.forward * Time.deltaTime * moveForce, ForceMode.Impulse);
        if (Input.GetKey(KeyCode.S))
            rb.AddForce(-transform.forward * Time.deltaTime * moveForce, ForceMode.Impulse);
        if (Input.GetKey(KeyCode.A))
            rb.AddForce(-transform.right * Time.deltaTime * moveForce, ForceMode.Impulse);
        if (Input.GetKey(KeyCode.D))
            rb.AddForce(transform.right * Time.deltaTime * moveForce, ForceMode.Impulse);

        if (Vector3.Magnitude(rb.velocity) > maxSPeed)
            rb.velocity = Vector3.Normalize(rb.velocity) * maxSPeed;


        if (cursorLocked)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Limits vertical rotation to avoid flipping

            cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }

        // Jumping logic
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false; // Ensure double jump prevention until grounded again
        }
    }
}
