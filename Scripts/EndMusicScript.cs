using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndMusicScript : MonoBehaviour
{
    [SerializeField] private AudioSource gunSonuMUSIC;
    [SerializeField] private Image fadeImage; // 👈 siyah UI Image
    [SerializeField] private float fadeDuration = 3f;

    void Start()
    {
        gunSonuMUSIC = GetComponent<AudioSource>();

        // Güvenlik: oyun başında siyah görünmesin
        Color c = fadeImage.color;
        c.a = 0f;
        fadeImage.color = c;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        gunSonuMUSIC.Play();
        StartCoroutine(FadeToBlack());
    }

    IEnumerator FadeToBlack()
    {
        float elapsed = 0f;
        Color c = fadeImage.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }

        c.a = 1f;
        fadeImage.color = c;

      
    }
}
