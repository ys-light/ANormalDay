using UnityEngine;

public class HeadBob : MonoBehaviour
{
    [Header("References")]
    public Transform cameraTransform;

    [Header("Bobbing Settings")]
    public float bobSpeed = 6f;        // Yürürken bob hýzý
    public float bobAmount = 0.06f;    // Bob yüksekliði
    public float smooth = 8f;          // Kameranýn target pozisyona yumuþak geçiþi

    private float defaultY;
    private float timer = 0f;

    void Start()
    {
        defaultY = cameraTransform.localPosition.y;
        Debug.Log("[Headbob] Started. Camera: " + cameraTransform.name);
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        bool isMoving = (horizontal != 0 || vertical != 0);

        Vector3 localPos = cameraTransform.localPosition;

        if (isMoving)
        {
            timer += Time.deltaTime * bobSpeed;
            float bob = Mathf.Sin(timer) * bobAmount;
            localPos.y = defaultY + bob;
        }
        else
        {
            // Hareket etmiyorsa kamerayý yumuþak biçimde eski yerine döndür
            timer = 0;
            localPos.y = Mathf.Lerp(localPos.y, defaultY, Time.deltaTime * smooth);
        }

        cameraTransform.localPosition = localPos;
    }
}
