using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerBallRandomChoice))]
public class PlayerBallMoving : MonoBehaviour
{
    [SerializeField] private GameObject _stick;

    private Rigidbody2D _currentPlayersBall;
    private Collider2D _currentPlayersBallCollider;

    private float _minX = -1.5f;
    private float _maxX = 1.5f;

    private float _smoothMovingTime = 0.1f;
    private Vector2 _touchPosition;

    private bool _firstStationary = true;

    private Camera _mainCamera;
    private PlayerBallRandomChoice _randomChoice;

    private Coroutine _smoothMovingCor;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _randomChoice = GetComponent<PlayerBallRandomChoice>();
    }

    private void Start()
    {
        NextBall();
    }

    private void Update()
    {
        if (_currentPlayersBall != null)
        {
            MoveObject();

            if (_currentPlayersBallCollider != null)
            {
                _stick.transform.position = new Vector3(_currentPlayersBall.transform.position.x, _currentPlayersBallCollider.bounds.min.y, _currentPlayersBall.transform.position.z);
            }
        }
    }

    private void MoveObject()
    {
        if (Input.touchCount > 0 && _currentPlayersBall != null)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosWorld = _mainCamera.ScreenToWorldPoint(touch.position);
            Vector2 targetPos = new Vector2(Mathf.Clamp(touchPosWorld.x, _minX, _maxX), _currentPlayersBall.transform.position.y);

            _touchPosition = touchPosWorld;

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (_smoothMovingCor == null)
                    {
                        _smoothMovingCor = StartCoroutine(SmoothMoving(_currentPlayersBall.gameObject, targetPos, _smoothMovingTime));
                    }

                    break;
                case TouchPhase.Stationary:
                    if (_smoothMovingCor == null && _firstStationary == true)
                    {
                        _firstStationary = false;
                        _smoothMovingCor = StartCoroutine(SmoothMoving(_currentPlayersBall.gameObject, targetPos, _smoothMovingTime));
                    }

                    break;
                case TouchPhase.Moved:
                    if (_smoothMovingCor != null)
                    {
                        StopCoroutine(_smoothMovingCor);
                        _smoothMovingCor = null;
                    }

                    _currentPlayersBall.transform.position = new Vector3(Mathf.Clamp(touchPosWorld.x, _minX, _maxX), _currentPlayersBall.transform.position.y, _currentPlayersBall.transform.position.z);
                    break;
                case TouchPhase.Ended:
                    _randomChoice.SpawnPos = targetPos;

                    _currentPlayersBall.bodyType = RigidbodyType2D.Dynamic;
                    _currentPlayersBall = null;
                    _currentPlayersBallCollider = null;

                    _stick.SetActive(false);

                    Invoke(nameof(NextBall), 0.5f);
                    _firstStationary = true;
                    break;
            }
        }
    }

    private void NextBall()
    {
        _currentPlayersBall = _randomChoice.NextBall().GetComponent<Rigidbody2D>();
        _currentPlayersBallCollider = _currentPlayersBall.GetComponent<Collider2D>();
        
        _currentPlayersBall.bodyType = RigidbodyType2D.Kinematic;
        _stick.SetActive(true);
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
