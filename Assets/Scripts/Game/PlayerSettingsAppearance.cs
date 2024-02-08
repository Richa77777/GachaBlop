using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSettingsAppearance : MonoBehaviour
{
    [SerializeField] private CanvasGroup _settingsCanvasGroup;
    [SerializeField] private float _animationDuration = 0.5f;

    private Coroutine _settingsCor;

    public void SwitchSettingsState(bool enable)
    {
        if (_settingsCor == null)
        {
            if (enable)
            {
                _settingsCor = StartCoroutine(EnableSettingsCor());
            }
            else if (!enable)
            {
                _settingsCor = StartCoroutine(DisableSettingsCor());
            }
        }
    }

    private IEnumerator EnableSettingsCor()
    {
        _settingsCanvasGroup.gameObject.SetActive(true);

        float startAlpha = 0f;
        float endAlpha = 1f;

        for (float t = 0; t < _animationDuration; t += Time.deltaTime)
        {
            _settingsCanvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, t / _animationDuration);
            yield return null;
        }

        _settingsCanvasGroup.alpha = endAlpha;
        _settingsCor = null;
    }

    private IEnumerator DisableSettingsCor()
    {
        float startAlpha = 1f;
        float endAlpha = 0f;

        for (float t = 0; t < _animationDuration; t += Time.deltaTime)
        {
            _settingsCanvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, t / _animationDuration);
            yield return null;
        }

        _settingsCanvasGroup.alpha = endAlpha;
        _settingsCanvasGroup.gameObject.SetActive(false);
        _settingsCor = null;
    }
}
