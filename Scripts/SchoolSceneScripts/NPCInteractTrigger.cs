using UnityEngine;
using TMPro;
using DialogueEditor;

public class NPCInteractTrigger : MonoBehaviour
{
    public TMP_Text interactText;
    public NPCConversation conversation;
    void Start()
    {
        interactText.gameObject.SetActive(false);
    }
    public void StartDialogue()
    {
        if (ConversationManager.Instance != null &&
            !ConversationManager.Instance.IsConversationActive)
        {
            ConversationManager.Instance.StartConversation(conversation);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactText.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactText.gameObject.SetActive(false);
        }
    }

}
