using UnityEngine;

[CreateAssetMenu(fileName = "FishData", menuName = "Fish/Fish Data")]
public class FishData : ScriptableObject
{
    [Header("Identitas")]
    public string fishName;
    public string fishNameEN;
    public string latinName;

    [Header("Info (Indonesian)")]
    public string habitat;
    public string description;
    public string conservationStatus;

    [Header("Info (English)")]
    public string habitatEN;
    public string descriptionEN;
    public string conservationStatusEN;

    [Header("Ukuran")]
    public float maxSizeCm;

    [Header("Visual")]
    public Sprite fishImage;

    [Header("Display 3D")]
    public int modelIndex = 0;

    public string GetName()
    {
        bool isEN = LanguageManager.Instance != null &&
                    LanguageManager.Instance.CurrentLanguage == Language.English;
        return isEN && !string.IsNullOrEmpty(fishNameEN) ? fishNameEN : fishName;
    }

    public string GetHabitat()
    {
        bool isEN = LanguageManager.Instance != null &&
                    LanguageManager.Instance.CurrentLanguage == Language.English;
        return isEN && !string.IsNullOrEmpty(habitatEN) ? habitatEN : habitat;
    }

    public string GetDescription()
    {
        bool isEN = LanguageManager.Instance != null &&
                    LanguageManager.Instance.CurrentLanguage == Language.English;
        return isEN && !string.IsNullOrEmpty(descriptionEN) ? descriptionEN : description;
    }

    public string GetStatus()
    {
        bool isEN = LanguageManager.Instance != null &&
                    LanguageManager.Instance.CurrentLanguage == Language.English;
        return isEN && !string.IsNullOrEmpty(conservationStatusEN) ? conservationStatusEN : conservationStatus;
    }
}