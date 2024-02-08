using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoseZone : MonoBehaviour
{
    public static Action OnLose;

    [SerializeField] private GameObject _loseTab;
    [SerializeField] private Image _loseTabBackground;
    [SerializeField] private GameObject _newRecordText;

    [SerializeField] private PlayerBallMoving _playerBallMoving;
    [SerializeField] private LayerMask _ballsLayer;

    [SerializeField] private float _boxCastSize = 0.35f;
    [SerializeField] private float _lineBlinkDuration = 1f;
    [SerializeField] private float _loseAnimationsDuration = 0.5f;

    private Collider2D _collider;
    private SpriteRenderer _spriteRenderer;
    private Coroutine _lineBlinkCor;
    private Coroutine _endLineBlinkCor;
    private Coroutine _loseCor;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        RaycastHit2D hit = Physics2D.BoxCast(new Vector2(transform.position.x, _collider.bounds.min.y), new Vector2(transform.localScale.x, _boxCastSize), 0, Vector2.down, 0f, _ballsLayer.value);

        if (hit)
        {
            if (_lineBlinkCor == null && _endLineBlinkCor == null)
            {
                _lineBlinkCor = StartCoroutine(LineBlink());
            }
        }
        else if (!hit)
        {
            if (_lineBlinkCor != null)
            {
                StopCoroutine(_lineBlinkCor);
                _lineBlinkCor = null;

                if (_endLineBlinkCor == null)
                {
                    _endLineBlinkCor = StartCoroutine(EndLineBlink());
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ball"))
        {
            Lose();
        }
    }

    private void Lose()
    {
        if (_loseCor == null)
        {
            _loseCor = StartCoroutine(LoseCor());
        }

        _playerBallMoving.enabled = false;
        OnLose?.Invoke();
    }

    private IEnumerator LoseCor()
    {
        #region Background
        Color startColor = _loseTabBackground.color;
        Color endColor = _loseTabBackground.color;
        startColor.a = 0f;

        _loseTabBackground.color = startColor;
        _loseTabBackground.gameObject.SetActive(true);

        for (float t = 0; t < _loseAnimationsDuration; t += Time.deltaTime)
        {
            _loseTabBackground.color = Color.Lerp(startColor, endColor, t / _loseAnimationsDuration);
            yield return null;
        }

        _loseTabBackground.color = endColor;
        #endregion

        #region LoseTab
        Vector3 startScale = Vector3.zero;
        Vector3 endScale = _loseTab.transform.localScale;

        _loseTab.transform.localScale = startScale;
        _loseTab.SetActive(true);

        for (float t = 0; t < _loseAnimationsDuration; t += Time.deltaTime)
        {
            _loseTab.transform.localScale = Vector3.Lerp(startScale, endScale, t / _loseAnimationsDuration);
            yield return null;
        }

        _loseTab.transform.localScale = endScale;
        #endregion

        #region NewRecord
        if (_newRecordText.activeInHierarchy == true)
        {
            endScale = Vector3.one;
            _newRecordText.transform.localScale = startScale;

            for (float t = 0; t < _loseAnimationsDuration; t += Time.deltaTime)
            {
                _newRecordText.transform.localScale = Vector3.Lerp(startScale, endScale, t / _loseAnimationsDuration);
                yield return null;
            }

            _newRecordText.transform.localScale = endScale;
        }
        #endregion
    }

    private IEnumerator LineBlink()
    {
        Color startColor = _spriteRenderer.color;
        Color endColor = _spriteRenderer.color;
        startColor.a = 0f;
        endColor.a = 1f;

        while (true)
        {
            for (float t = 0; t < _lineBlinkDuration; t += Time.deltaTime)
            {
                _spriteRenderer.color = Color.Lerp(startColor, endColor, t / _lineBlinkDuration);
                yield return null;
            }

            _spriteRenderer.color = endColor;
            yield return new WaitForSeconds(0.3f);

            for (float t = 0; t < _lineBlinkDuration; t += Time.deltaTime)
            {
                _spriteRenderer.color = Color.Lerp(endColor, startColor, t / _lineBlinkDuration);
                yield return null;
            }

            _spriteRenderer.color = startColor;
            yield return new WaitForSeconds(0.3f);
        }
    }

    private IEnumerator EndLineBlink()
    {
        Color startColor = _spriteRenderer.color;
        Color currentColor = _spriteRenderer.color;
        startColor.a = 0f;

        for (float t = 0; t < _lineBlinkDuration; t += Time.deltaTime)
        {
            _spriteRenderer.color = Color.Lerp(currentColor, startColor, t / _lineBlinkDuration);
            yield return null;
        }

        _spriteRenderer.color = startColor;
        _endLineBlinkCor = null;
    }
}
