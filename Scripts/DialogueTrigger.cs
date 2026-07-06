using DialogueEditor;
using UnityEngine;
using TMPro;

public class DialogueTrigger : MonoBehaviour
{
    public NPCConversation conversation;
    public TMP_Text interactText;

    public bool playerInside = false;

    void Update()
    {
        if (!ConversationManager.Instance.IsConversationActive && playerInside)
        {
            interactText.gameObject.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                StartDialogue();
                interactText.gameObject.SetActive(false);
            }
        }
        else
        {
            interactText.gameObject.SetActive(false);
        }
    }

    public void StartDialogue()
    {
        if (ConversationManager.Instance != null &&
            !ConversationManager.Instance.IsConversationActive)
        {
            ConversationManager.Instance.StartConversation(conversation);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInside = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInside = false;
    }
}
