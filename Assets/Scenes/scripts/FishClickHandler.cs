using UnityEngine;
using UnityEngine.EventSystems;

public class FishClickHandler : MonoBehaviour, IPointerClickHandler
{
    public FishData fishData;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Diklik: " + gameObject.name);

        if (FishInfoUI.Instance == null)
        {
            Debug.LogError("FishInfoUI.Instance null! Pastikan FishInfoUI ada di scene.");
            return;
        }
        if (fishData == null)
        {
            Debug.LogError("FishData belum di-assign di prefab: " + gameObject.name);
            return;
        }

        GetComponent<FishSwim2D>()?.PauseSwimming(3f);
        FishInfoUI.Instance.ShowInfo(fishData);
    }
}