using UnityEngine;

public class DoorInteract : MonoBehaviour
{
    public float openAngle = 90f;
    public float openSpeed = 3f;

    bool isOpen = false;

    Quaternion closedRot;
    Quaternion openRot;

    void Start()
    {
        closedRot = transform.localRotation;
        openRot = Quaternion.Euler(0f, openAngle, 0f) * closedRot;
    }

    void Update()
    {
        Quaternion target = isOpen ? openRot : closedRot;
        transform.localRotation = Quaternion.Lerp(
            transform.localRotation,
            target,
            Time.deltaTime * openSpeed
        );
    }

    public void ToggleDoor()
    {
        Debug.Log("KAPI TETÝKLENDÝ");
        isOpen = !isOpen;
    }
}
