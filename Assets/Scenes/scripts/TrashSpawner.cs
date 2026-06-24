using UnityEngine;
using System.Collections;

public class TrashSpawner : MonoBehaviour
{
    public GameObject[] trashPrefabs;
    public RectTransform aquariumPanel;
    public float spawnInterval = 10f;
    public float fallSpeed     = 80f;
    public float bottomPadding = 60f;

    void Start() => StartCoroutine(SpawnLoop());

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnTrash();
        }
    }

    void SpawnTrash()
    {
        if (trashPrefabs == null || trashPrefabs.Length == 0 || aquariumPanel == null) return;

        GameObject prefab = trashPrefabs[Random.Range(0, trashPrefabs.Length)];
        GameObject trash  = Instantiate(prefab, aquariumPanel);

        RectTransform rt = trash.GetComponent<RectTransform>();
        rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);

        Rect r = aquariumPanel.rect;
        float startX = Random.Range(r.xMin + 60f, r.xMax - 60f);
        rt.anchoredPosition = new Vector2(startX, r.yMax + 50f);

        TrashItem item = trash.GetComponent<TrashItem>();
        if (item == null) item = trash.AddComponent<TrashItem>();
        item.Init(aquariumPanel, fallSpeed, bottomPadding);
    }
}