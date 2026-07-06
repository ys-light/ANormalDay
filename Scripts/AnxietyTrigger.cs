using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AnxietyTrigger : MonoBehaviour
{
    [SerializeField] private float anxietyIncreaseAmount = 25f;
    [SerializeField] private AudioSource anksiyeteSesi;
    void Start()
    {
        GetComponent<Collider>().isTrigger = true;
        anksiyeteSesi = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        AnxietyManager.Instance.EnterTrigger();
        AnxietyManager.Instance.IncreaseAnxiety(anxietyIncreaseAmount);
        anksiyeteSesi.Play();
      
    }
    void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        AnxietyManager.Instance.EnterTrigger(); // ⭐ yeni sahnede hayat kurtarır
       
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        AnxietyManager.Instance.ExitTrigger();
        anksiyeteSesi.Pause();
    }
}
