using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(PlayerBallRandomChoice))]
public class PlayerBallMoving : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _stick;
    [SerializeField] private AudioClip _fallSound;
    [SerializeField] private Collider2D _loseZone;

    private Rigidbody2D _currentBall;
    private Collider2D _ballCollider;
    private SpriteRenderer _ballSpriteRenderer;
    private int _ballStartSortingOrder = 0;

    private float _minX = -1.3f;
    private float _maxX = 1.3f;
    private float _smoothMovingTime = 0.1f;
    private bool _firstStationary = true;

    private Vector2 _touchPosition;


    private Camera _mainCamera;
    private PlayerBallRandomChoice _randomChoice;
    private AudioSource _audioSource;

    private Coroutine _smoothMovingCor;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _randomChoice = GetComponent<PlayerBallRandomChoice>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        NextBall();
    }

    private void Update()
    {
        if (_currentBall != null)
        {
            MoveObject();

            if (_ballCollider != null)
            {
                _stick.transform.parent.transform.position = new Vector3(_currentBall.transform.position.x, _ballCollider.bounds.min.y, _currentBall.transform.position.z);
            }
        }
    }

    private void OnEnable()
    {
        if (_ballCollider != null)
        {
            _stick.transform.parent.transform.position = new Vector3(_currentBall.transform.position.x, _ballCollider.bounds.min.y, _currentBall.transform.position.z);
        }
    }

    private void OnDisable()
    {
        if (_ballCollider != null)
        {
            _stick.transform.parent.transform.position = new Vector3(_currentBall.transform.position.x, _ballCollider.bounds.min.y, _currentBall.transform.position.z);
        }
    }

    private void MoveObject()
    {
        if (Input.touchCount > 0 && _currentBall != null)
        {
            Touch touch = Input.GetTouch(0);

            if (!IsTouchOverUI(touch))
            {
                Vector2 touchPosWorld = _mainCamera.ScreenToWorldPoint(touch.position);
                Vector2 targetPos = new Vector2(Mathf.Clamp(touchPosWorld.x, _minX, _maxX), _currentBall.transform.position.y);

                _touchPosition = touchPosWorld;

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        if (_smoothMovingCor == null)
                        {
                            _smoothMovingCor = StartCoroutine(SmoothMoving(_currentBall.gameObject, targetPos, _smoothMovingTime));
                        }
                        break;
                    case TouchPhase.Stationary:
                        if (_smoothMovingCor == null && _firstStationary == true)
                        {
                            _firstStationary = false;
                            _smoothMovingCor = StartCoroutine(SmoothMoving(_currentBall.gameObject, targetPos, _smoothMovingTime));
                        }

                        break;
                    case TouchPhase.Moved:
                        if (_smoothMovingCor != null)
                        {
                            StopCoroutine(_smoothMovingCor);
                            _smoothMovingCor = null;
                        }

                        _currentBall.transform.position = new Vector3(Mathf.Clamp(targetPos.x, _minX, _maxX), _currentBall.transform.position.y, _currentBall.transform.position.z);
                        break;
                    case TouchPhase.Ended:
                        _randomChoice.SpawnPos = targetPos;
                        _audioSource.PlayOneShot(_fallSound);

                        _currentBall.bodyType = RigidbodyType2D.Dynamic;
                        _currentBall = null;
                        _ballCollider.enabled = true;
                        _ballCollider = null;
                        _ballSpriteRenderer.sortingOrder = _ballStartSortingOrder;

                        _stick.transform.parent.gameObject.SetActive(false);
                        _loseZone.enabled = false;

                        Invoke(nameof(NextBall), 0.5f);

                        _firstStationary = true;

                        break;
                }
            }
        }
    }

    private bool IsTouchOverUI(Touch touch)
    {
        if (EventSystem.current != null)
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = touch.position;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            return results.Count > 0;
        }

        return false;
    }

    private void NextBall()
    {
        _currentBall = _randomChoice.NextBall().GetComponent<Rigidbody2D>();
        _ballCollider = _currentBall.GetComponent<Collider2D>();
        _ballSpriteRenderer = _currentBall.GetComponent<SpriteRenderer>();
        _ballStartSortingOrder = _ballSpriteRenderer.sortingOrder;

        _ballSpriteRenderer.sortingOrder = _stick.sortingOrder + 1;

        _currentBall.bodyType = RigidbodyType2D.Kinematic;
        _ballCollider.enabled = false;

        if (_ballCollider != null)
        {
            _stick.transform.parent.transform.position = new Vector3(_currentBall.transform.position.x, _ballCollider.bounds.min.y, _currentBall.transform.position.z);
        }

        _stick.transform.parent.gameObject.SetActive(true);
        _loseZone.enabled = true;
    }

    private IEnumerator SmoothMoving(GameObject obj, Vector2 targetPos, float duration)
    {
        Vector2 startPos = obj.transform.position;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            obj.transform.position = Vector2.Lerp(startPos, targetPos, t / duration);
            yield return null;
        }

        obj.transform.position = targetPos;
        _smoothMovingCor = null;
    }
}
