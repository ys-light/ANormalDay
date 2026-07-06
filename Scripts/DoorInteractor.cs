using UnityEngine;
using TMPro;
using DialogueEditor;

public class DoorInteractor : MonoBehaviour
{
    public float rayDistance = 4f;
    public TextMeshProUGUI interactText;

    DoorInteract currentDoor;

    void Start()
    {
        interactText.gameObject.SetActive(false);
    }

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red);
        // 🔒 UI MODE → raycast kapalı
        if (CursorManager.Instance != null && CursorManager.Instance.IsUIMode())
        {
            interactText.gameObject.SetActive(false);
            currentDoor = null;
            return;
        }

    

        bool hitDoor = false;

        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
            if (hit.collider.CompareTag("Door"))
            {
                DoorInteract door =
                    hit.collider.GetComponentInParent<DoorInteract>();

                if (door != null)
                {
                    hitDoor = true;
                    currentDoor = door;
                    interactText.gameObject.SetActive(true);
                }
            }
        }

        if (!hitDoor)
        {
            interactText.gameObject.SetActive(false);
            currentDoor = null;
        }

        // E ile aç
        if (currentDoor != null && Input.GetKeyDown(KeyCode.E))
        {
            currentDoor.ToggleDoor();
        }
    }
}
