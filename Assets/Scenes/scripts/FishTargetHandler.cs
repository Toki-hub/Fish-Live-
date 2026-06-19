using UnityEngine;
using Vuforia;

public class FishTargetHandler : MonoBehaviour
{
    [SerializeField] private GameObject aquariumPanel;
    [SerializeField] private GameObject fishPrefab;

    private const int MaxFishCount = 3;
    private int spawnedCount = 0;
    private ObserverBehaviour observer;

    void Start()
    {
        observer = GetComponent<ObserverBehaviour>();
        if (observer != null)
            observer.OnTargetStatusChanged += OnTargetStatusChanged;
    }

    void OnDestroy()
    {
        if (observer != null)
            observer.OnTargetStatusChanged -= OnTargetStatusChanged;
    }

    private void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus targetStatus)
    {
        var status = targetStatus.Status;
        if (status == Status.TRACKED ||
            status == Status.EXTENDED_TRACKED ||
            status == Status.LIMITED)
        {
            OnTrackingFound();
        }
    }

    private void OnTrackingFound()
    {
        if (aquariumPanel != null)
            aquariumPanel.SetActive(true);

        if (fishPrefab != null && spawnedCount < MaxFishCount)
        {
            GameObject fish = Instantiate(fishPrefab, aquariumPanel.transform);
            // Spawn at random position so they don't stack
            fish.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                Random.Range(-300f, 300f),
                Random.Range(-100f, 100f)
            );
            spawnedCount++;
        }
    }
}