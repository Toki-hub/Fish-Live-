using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LanguageButtonUI : MonoBehaviour
{
    public TMP_Text buttonLabel;

    void Start()
    {
        UpdateLabel();
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        LanguageManager.Instance.ToggleLanguage();
        UpdateLabel();
    }

    void UpdateLabel()
    {
        buttonLabel.text = LanguageManager.Instance.CurrentLanguage
            == Language.English ? "EN" : "ID";
    }
}