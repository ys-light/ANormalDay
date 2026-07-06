using UnityEngine;
using System.Collections;

public class DoorTeleport : MonoBehaviour
{
    public Transform player;
    public FadeManager fadeManager;

    public float targetZ = 231f;
    public float rayDistance = 5f;

    bool triggered = false;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (triggered) return;

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
            // Tag kontrolü
            if (hit.collider.CompareTag("Door") && Input.GetMouseButtonDown(0)) // Sol týk
            {
                triggered = true;
                StartCoroutine(TeleportSequence());
            }
        }
    }

    IEnumerator TeleportSequence()
    {
        // Fade to black
        yield return StartCoroutine(fadeManager.FadeIn());

        // Teleport
        Vector3 pos = player.position;
        pos.z = targetZ;
        player.position = pos;

        // Fade back
        yield return StartCoroutine(fadeManager.FadeOut());

        triggered = false; // Ýstersen tekrar kullanýlabilir yap
    }
}
