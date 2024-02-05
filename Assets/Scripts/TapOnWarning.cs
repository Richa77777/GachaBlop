using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TapOnWarning : MonoBehaviour
{
    [SerializeField] private CanvasGroup _warningCanvas;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Ended)
            {
                StartCoroutine(StartGame());
            }
        }
    }

    private IEnumerator StartGame()
    {
        for (float t = 0; t < 1f; t += Time.deltaTime)
        {
            _warningCanvas.alpha = Mathf.Lerp(1f, 0f, t / 1f);
            yield return null;
        }

        _warningCanvas.alpha = 0f;

        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene("Menu");
    }
}
