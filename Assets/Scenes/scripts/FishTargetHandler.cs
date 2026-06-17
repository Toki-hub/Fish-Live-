using UnityEngine;
using UnityEngine.UI;
using Vuforia;
using TMPro;

public class FishTargetHandler : MonoBehaviour
{
    [Header("Prefab & Container")]
    public GameObject    fishPrefab;
    public RectTransform aquariumPanel;

    [Header("Batas spawn")]
    public int maxCount = 3;

    [Header("Feedback (opsional)")]
    public TMP_Text feedbackText;

    private ObserverBehaviour observer;
    private int  spawnedCount = 0;
    private bool wasTracked   = false;

    void Start()
    {
        observer = GetComponent<ObserverBehaviour>();
        if (observer != null)
            observer.OnTargetStatusChanged += OnStatusChanged;

        // Debug: cek assignment
        if (fishPrefab    == null) Debug.LogError($"{name}: fishPrefab belum di-assign!");
        if (aquariumPanel == null) Debug.LogError($"{name}: aquariumPanel belum di-assign!");
    }

    void OnStatusChanged(ObserverBehaviour b, TargetStatus status)
    {
        bool isTracked = status.Status == Status.TRACKED
                      || status.Status == Status.EXTENDED_TRACKED;

        if (isTracked && !wasTracked)
        {
            wasTracked = true;   // ← set SEKARANG, cegah race condition
            OnNewScan();
        }
        else if (!isTracked)
        {
            wasTracked = false;
        }
    }

    void OnNewScan()
    {
        if (spawnedCount >= maxCount)
        {
            ShowFeedback($"❌ Batas maksimal tercapai! ({maxCount}/{maxCount})");
            return;
        }

        spawnedCount++;          // ← increment DULU sebelum spawn
        SpawnOneFish();
        ShowFeedback(spawnedCount < maxCount
            ? $"🐟 Ikan ditambahkan! ({spawnedCount}/{maxCount})"
            : $"✅ Akuarium penuh! ({spawnedCount}/{maxCount})");
    }

    void SpawnOneFish()
    {
        if (fishPrefab == null || aquariumPanel == null) return;

        GameObject fish = Instantiate(fishPrefab, aquariumPanel);
        RectTransform rt = fish.GetComponent<RectTransform>();

        Rect r = aquariumPanel.rect;
        rt.anchoredPosition = new Vector2(
            Random.Range(r.xMin + 120, r.xMax - 120),
            Random.Range(r.yMin + 80,  r.yMax - 80)
        );

        fish.transform.localScale = Vector3.zero;
        StartCoroutine(ScaleIn(fish.transform));
    }

    System.Collections.IEnumerator ScaleIn(Transform t)
    {
        float elapsed = 0f, duration = 0.4f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            t.localScale = Vector3.one * Mathf.SmoothStep(0f, 1f, elapsed / duration);
            yield return null;
        }
        t.localScale = Vector3.one;
    }

    void ShowFeedback(string msg)
    {
        if (feedbackText == null) return;
        feedbackText.text = msg;
        feedbackText.gameObject.SetActive(true);
        CancelInvoke(nameof(HideFeedback));
        Invoke(nameof(HideFeedback), 2f);
    }
    void HideFeedback()
    {
        if (feedbackText != null)
            feedbackText.gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        if (observer != null)
            observer.OnTargetStatusChanged -= OnStatusChanged;
    }
}