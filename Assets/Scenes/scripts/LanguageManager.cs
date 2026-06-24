using UnityEngine;
using System;

public enum Language { Indonesian, English }

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager Instance { get; private set; }
    public Language CurrentLanguage { get; private set; } = Language.Indonesian;

    public static event Action OnLanguageChanged;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;

        int saved = PlayerPrefs.GetInt("Language", (int)Language.Indonesian);
        CurrentLanguage = (Language)saved;
    }

    public void ToggleLanguage()
    {
        CurrentLanguage = CurrentLanguage == Language.Indonesian
            ? Language.English : Language.Indonesian;

        PlayerPrefs.SetInt("Language", (int)CurrentLanguage);
        OnLanguageChanged?.Invoke();
    }
}