using UnityEngine;
using TMPro;
using DialogueEditor;

public class NPCInteractor : MonoBehaviour
{
    public float rayDistance = 4f;
    public TextMeshProUGUI interactText;

    DialogueTrigger currentNPC;

    void Start()
    {
        interactText.gameObject.SetActive(false);
    }

    void Update()
    {

        interactText.gameObject.SetActive(false);
        currentNPC = null;

        Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.green);

        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
            if (hit.collider.CompareTag("NPC"))
            {
                DialogueTrigger npc =
                    hit.collider.GetComponentInParent<DialogueTrigger>();

                if (npc != null && npc.playerInside)
                {
                    currentNPC = npc;
                    interactText.gameObject.SetActive(true);
                    interactText.text = "E - Konuş";
                }
            }

        }

        // 🎮 E BASINCA KONUŞ
        if (currentNPC != null && Input.GetKeyDown(KeyCode.E))
        {
            ConversationManager.Instance.StartConversation(currentNPC.conversation);
            interactText.gameObject.SetActive(false);
        }

    }
}
