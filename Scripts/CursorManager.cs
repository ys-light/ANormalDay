using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance;

    [Header("UI")]
    public GameObject crosshair;

    bool uiMode = false;
    bool isMainInstance = false;

    void Awake()
    {
        // İlk gelen ana instance olur
        if (Instance == null)
        {
            Instance = this;
            isMainInstance = true;
        }
        else
        {
            // ❗ Diğerleri SİLİNMEZ, sadece pasif olur
            enabled = false;
            return;
        }
    }

    void Start()
    {
        if (!isMainInstance) return;

        SetGameplayMode();
    }

    public void SetGameplayMode()
    {
        if (!isMainInstance) return;

        uiMode = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (crosshair != null)
            crosshair.SetActive(true);
    }

    public void SetUIMode()
    {
        if (!isMainInstance) return;

        uiMode = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (crosshair != null)
            crosshair.SetActive(false);
    }

    public bool IsUIMode()
    {
        return uiMode;
    }
}
