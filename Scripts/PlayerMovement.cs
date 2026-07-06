using UnityEngine;
using DialogueEditor;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Move Settings")]
    [SerializeField] private float baseMoveSpeed = 5f;

    [Header("Mouse Settings")]
    [SerializeField] private float mouseSensitivity = 120f;
    [SerializeField] private Transform cameraHolder;

    [Header("UI")]
    [SerializeField] private GameObject crosshair;

    private float xRotation = 0f;
    private Vector3 movementInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        // 🔒 DİYALOG AÇIKSA → TAM KİLİT
        if (ConversationManager.Instance != null &&
            ConversationManager.Instance.IsConversationActive)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (crosshair != null)
                crosshair.SetActive(false);

            movementInput = Vector3.zero;
            return;
        }

        // 🔓 NORMAL GAMEPLAY
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (crosshair != null)
            crosshair.SetActive(true);

        // ----- MOUSE LOOK -----
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -85f, 85f);
        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // ----- MOVEMENT -----
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        movementInput =
            (transform.forward * vertical + transform.right * horizontal).normalized;
    }

    void FixedUpdate()
    {
        // 🔒 DİYALOG AÇIKSA → HAREKET YOK
        if (ConversationManager.Instance != null &&
            ConversationManager.Instance.IsConversationActive)
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
            return;
        }

        Vector3 targetVelocity = movementInput * baseMoveSpeed;
        rb.linearVelocity =
            new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.z);
    }
}
