using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class AnxietyManager : MonoBehaviour
{
    public static AnxietyManager Instance;

    [Header("Anxiety Values")]
    public float maxAnxiety = 100f;
    public float currentAnxiety = 0f;

    [Header("Visual Limits")]
    public float maxHueShift = 120f;
    public float maxChromatic = 2.5f;

    [Header("Hue Oscillation")]
    public float hueOscillationSpeed = 3f;
    public float hueOscillationStrength = 0.8f;

    [Header("Vignette")]
    public float maxVignetteIntensity = 0.35f;
    public float maxVignetteSmoothness = 1f;
    public float vignetteStepPerTrigger = 0.08f;

    [Header("Smooth Settings")]
    public float baseVisualSmoothSpeed = 6f;
    public float panicVisualSmoothSpeed = 10f;

    [Header("Post Process")]
    public Volume volume;

    [Header("UI")]
    public Slider anxietySlider;

    [Header("Decay")]
    public float anxietyDecreaseRate = 8f;
    public float delayBeforeDecay = 5f;

    ColorAdjustments colorAdjustments;
    ChromaticAberration chromatic;
    Vignette vignette;

    float visualSmoothSpeed;
    float vignetteBoost = 0f;

    bool isInTrigger = false;
    float lastExitTime = -999f;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        volume.profile.TryGet(out colorAdjustments);
        volume.profile.TryGet(out chromatic);
        volume.profile.TryGet(out vignette);

        visualSmoothSpeed = baseVisualSmoothSpeed;

        if (anxietySlider != null)
        {
            anxietySlider.minValue = 0f;
            anxietySlider.maxValue = 1f;
            anxietySlider.value = 0f;
        }
    }

    void Update()
    {
        if (colorAdjustments == null || chromatic == null || vignette == null)
            return;

        // 🔻 SADECE box DIŞINDAYSAN ve 5 sn geçtiyse düş
        if (!isInTrigger && currentAnxiety > 0f)
        {
            if (Time.time - lastExitTime >= delayBeforeDecay)
            {
                currentAnxiety -= anxietyDecreaseRate * Time.deltaTime;
                currentAnxiety = Mathf.Clamp(currentAnxiety, 0f, maxAnxiety);

                vignetteBoost = Mathf.MoveTowards(vignetteBoost, 0f, Time.deltaTime * 0.1f);
            }
        }

        float t = currentAnxiety / maxAnxiety;
        float visualT = Mathf.Pow(t, 3.2f);

        if (anxietySlider != null)
            anxietySlider.value = Mathf.Lerp(anxietySlider.value, t, Time.deltaTime * 8f);

        // 🎨 Hue dalgalanması
        float baseHue = Mathf.Lerp(0f, maxHueShift, visualT);
        float oscillation = Mathf.Sin(Time.time * hueOscillationSpeed) * baseHue * hueOscillationStrength;
        float targetHue = oscillation;

        float targetChromatic = Mathf.Lerp(0f, maxChromatic, visualT);

        // 🩸 Vignette
        float targetVignetteIntensity =
            Mathf.Lerp(0f, maxVignetteIntensity, visualT) + vignetteBoost;

        float targetVignetteSmoothness =
            Mathf.Lerp(0f, maxVignetteSmoothness, visualT);

        visualSmoothSpeed = (currentAnxiety > maxAnxiety * 0.85f)
            ? panicVisualSmoothSpeed
            : baseVisualSmoothSpeed;

        colorAdjustments.hueShift.value = Mathf.Lerp(
            colorAdjustments.hueShift.value,
            targetHue,
            Time.deltaTime * visualSmoothSpeed
        );

        float chromaNoise = Mathf.Sin(Time.time * 12f) * 0.15f * visualT;

        chromatic.intensity.value = Mathf.Lerp(
            chromatic.intensity.value,
            targetChromatic + chromaNoise,
            Time.deltaTime * visualSmoothSpeed
        );

        vignette.intensity.value = Mathf.Lerp(
            vignette.intensity.value,
            targetVignetteIntensity,
            Time.deltaTime * visualSmoothSpeed
        );

        vignette.smoothness.value = Mathf.Lerp(
            vignette.smoothness.value,
            targetVignetteSmoothness,
            Time.deltaTime * visualSmoothSpeed
        );
    }

    // 📈 Yeni korku alanına girildiğinde
    public void IncreaseAnxiety(float amount)
    {
        currentAnxiety = Mathf.Clamp(currentAnxiety + amount, 0f, maxAnxiety);
        vignetteBoost = Mathf.Clamp(vignetteBoost + vignetteStepPerTrigger, 0f, 0.5f);
    }

    public void EnterTrigger()
    {
        isInTrigger = true;
    }

    public void ExitTrigger()
    {
        isInTrigger = false;
        lastExitTime = Time.time; // ⏱ çıkış anı
    }
}
