using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class FishInfoUI : MonoBehaviour
{
    public static FishInfoUI Instance { get; private set; }

    [Header("Panel")]
    public GameObject panel;
    public GameObject closeOverlay;   // ← tambah ini

    [Header("Text Fields")]
    public TMP_Text fishNameText;
    public TMP_Text latinNameText;
    public TMP_Text habitatText;
    public TMP_Text descriptionText;
    public TMP_Text conservationText;
    public TMP_Text sizeText;

    [Header("Animasi")]
    public float animDuration = 0.3f;

    private RectTransform panelRT;
    private Coroutine currentAnim;

    void Awake() => Instance = this;

    void Start()
    {
        panelRT = panel.GetComponent<RectTransform>();
        panel.SetActive(false);

        // Setup CloseOverlay button
        if (closeOverlay != null)
        {
            closeOverlay.SetActive(false);
            Button btn = closeOverlay.GetComponent<Button>();
            if (btn != null)
                btn.onClick.AddListener(HideInfo);
        }
    }

public void ShowInfo(FishData data)
{
    if (data == null) return;

    fishNameText.text     = data.fishName;
    latinNameText.text    = $"<i>{data.latinName}</i>";
    habitatText.text      = $"Habitat: {data.habitat}";
    descriptionText.text  = data.description;
    conservationText.text = $"Status: {data.conservationStatus}";
    sizeText.text         = $"Size maks: {data.maxSizeCm} cm";

    FishModelDisplay.Instance?.ShowModel(data.modelIndex);
    FishModelDisplay.Instance?.displayCamera.gameObject.SetActive(true); // ← ADD

    if (closeOverlay != null)
        closeOverlay.SetActive(true);

    if (currentAnim != null) StopCoroutine(currentAnim);
    panel.SetActive(true);
    currentAnim = StartCoroutine(SlideIn());
}

public void HideInfo()
{
    FishModelDisplay.Instance?.HideModel();
    FishModelDisplay.Instance?.displayCamera.gameObject.SetActive(false); // ← ADD

    if (closeOverlay != null)
        closeOverlay.SetActive(false);

    if (currentAnim != null) StopCoroutine(currentAnim);
    currentAnim = StartCoroutine(SlideOut());
}

    IEnumerator SlideIn()
    {
        float elapsed = 0f;
        float targetY = 150f;
        CanvasGroup cg = panel.GetComponent<CanvasGroup>();
        if (cg == null) cg = panel.AddComponent<CanvasGroup>();

        while (elapsed < animDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / animDuration);
            panelRT.anchoredPosition = new Vector2(
                panelRT.anchoredPosition.x, Mathf.Lerp(-300f, targetY, t));
            cg.alpha = t;
            yield return null;
        }
        cg.alpha = 1f;
        panelRT.anchoredPosition = new Vector2(panelRT.anchoredPosition.x, targetY);
    }

    IEnumerator SlideOut()
    {
        float elapsed = 0f;
        float startY  = panelRT.anchoredPosition.y;
        CanvasGroup cg = panel.GetComponent<CanvasGroup>();
        if (cg == null) cg = panel.AddComponent<CanvasGroup>();

        while (elapsed < animDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / animDuration);
            panelRT.anchoredPosition = new Vector2(
                panelRT.anchoredPosition.x, Mathf.Lerp(startY, -300f, t));
            cg.alpha = 1f - t;
            yield return null;
        }
        panel.SetActive(false);
    }
}