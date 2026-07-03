using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using System.Collections;

[RequireComponent(typeof(UIDocument))]
public class LanguageSelector : MonoBehaviour
{
    private Button _btnEnglish;
    private Button _btnKorean;
    
    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        _btnEnglish = root.Q<Button>("English");
        _btnKorean = root.Q<Button>("Korean");

        _btnEnglish.clicked += OnClickEnglish;
        _btnKorean.clicked += OnClickKorean;
    }

    private void OnDisable()
    {
        if (_btnEnglish != null) _btnEnglish.clicked -= OnClickEnglish;
        if (_btnKorean != null) _btnKorean.clicked -= OnClickKorean;
    }
    
    private void OnClickEnglish() => StartCoroutine(CoSetLocaleRoutine("en-US"));
    private void OnClickKorean() => StartCoroutine(CoSetLocaleRoutine("ko-KR"));
    
    private IEnumerator CoSetLocaleRoutine(string localeCode)
    {
        yield return LocalizationSettings.InitializationOperation;

        Locale locale = LocalizationSettings.AvailableLocales.GetLocale(localeCode);

        if (locale == null)
        {
            yield break;
        }

        LocalizationSettings.SelectedLocale = locale;
    }
}
