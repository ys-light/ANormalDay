using UnityEngine;

public class RaycastPickup : MonoBehaviour
{
    public float rayDistance = 5f;
    public float moveSpeed = 8f;
    public float surfaceOffset = 0.2f;
    public Transform holdPoint;

    Rigidbody heldRb;
    Vector3 targetPos;

    enum State { None, Holding, Dropping }
    State state = State.None;

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red);

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (state == State.None)
                TryPickup(ray);
            else if (state == State.Holding)
                StartDrop(ray);
        }

        if (heldRb != null)
            MoveObject();
    }

    void TryPickup(Ray ray)
    {
        if (!Physics.Raycast(ray, out RaycastHit hit, rayDistance))
            return;

        // Sadece Interactable tag'li objeler
        if (!hit.collider.CompareTag("Interactable"))
            return;

        Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
        if (rb == null)
            return;

        // Temiz başlangıç
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        heldRb = rb;
        heldRb.useGravity = false;
        heldRb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        heldRb.interpolation = RigidbodyInterpolation.Interpolate;
        heldRb.linearDamping = 10f;

        targetPos = holdPoint.position;
        state = State.Holding;
    }

    void StartDrop(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
            targetPos = hit.point + hit.normal * surfaceOffset;
        else
            targetPos = transform.position + transform.forward * rayDistance;

        state = State.Dropping;
    }

    void MoveObject()
    {
        // Holding durumunda hedef sürekli holdPoint
        if (state == State.Holding)
            targetPos = holdPoint.position;

        Vector3 dir = targetPos - heldRb.position;
        heldRb.linearVelocity = dir * moveSpeed;

        // Drop hedefe ulaşınca bırak
        if (state == State.Dropping && dir.magnitude < 0.15f)
            FinishDrop();
    }

    void FinishDrop()
    {
        heldRb.linearVelocity = Vector3.zero;
        heldRb.angularVelocity = Vector3.zero;
        heldRb.linearDamping = 0f;
        heldRb.useGravity = true;
        heldRb = null;
        state = State.None;
    }
}
