using System.Collections;
using UnityEngine;

public class BoxShaking : MonoBehaviour
{
    [SerializeField] private GameObject _box;
    [SerializeField] private GameObject _loseZone;
    [SerializeField] private PlayerBallMoving _ballMoving;

    private float _duration = 1.0f;
    private float _magnitude = 0.5f;

    Vector3 _originalPos;
    Quaternion _originalRot;

    private void Awake()
    {
        _originalPos = _box.transform.position;
        _originalRot = _box.transform.rotation;
    }

    [ContextMenu("Shake")]
    public void ShakeBox()
    {
        StartCoroutine(BoxShakeCor(_duration, _magnitude));
        ApplyForcesToObjects(5.0f, 10.0f, 5.0f);

        _loseZone.SetActive(false);
        _ballMoving.enabled = false;

        Invoke(nameof(EnableBallMoving), _duration);
        Invoke(nameof(EnableLoseZone), 5f);
    }

    private IEnumerator BoxShakeCor(float duration, float magnitude)
    {
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = _originalPos.x + Mathf.PerlinNoise(Time.time * 20, 0) * magnitude - magnitude / 2.0f;
            float y = _originalPos.y + Mathf.PerlinNoise(0, Time.time * 20) * magnitude - magnitude / 2.0f;
            float z = _originalPos.z + Mathf.PerlinNoise(Time.time * 20, Time.time * 20) * magnitude - magnitude / 2.0f;

            _box.transform.position = new Vector3(x, y, z);

            float rotationX = _originalRot.eulerAngles.x + Mathf.PerlinNoise(Time.time * 10, 0) * 20 - 10;
            float rotationY = _originalRot.eulerAngles.y + Mathf.PerlinNoise(0, Time.time * 10) * 20 - 10;
            float rotationZ = _originalRot.eulerAngles.z + Mathf.PerlinNoise(Time.time * 10, Time.time * 10) * 20 - 10;

            _box.transform.rotation = Quaternion.Euler(rotationX, rotationY, rotationZ);

            elapsed += Time.deltaTime;

            yield return null;
        }

        StartCoroutine(ReturnToOriginalPos(_originalPos, _originalRot, 0.1f));
    }

    private IEnumerator ReturnToOriginalPos(Vector3 originalPos, Quaternion originalRot, float returnDuration)
    {
        Vector3 startPosition = _box.transform.position;
        Quaternion startRotation = _box.transform.rotation;

        for (float elapsed = 0; elapsed < returnDuration; elapsed += Time.deltaTime)
        {
            _box.transform.position = Vector3.Lerp(startPosition, originalPos, elapsed / returnDuration);
            _box.transform.rotation = Quaternion.Slerp(startRotation, originalRot, elapsed / returnDuration);

            yield return null;
        }

        _box.transform.position = originalPos;
        _box.transform.rotation = originalRot;
    }


    private void EnableLoseZone()
    {
        _loseZone.SetActive(true);
    }    

    private void EnableBallMoving()
    {
        _ballMoving.enabled = true;
    }

    private void ApplyForcesToObjects(float radius, float verticalForceMagnitude, float horizontalForceMagnitude)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_box.transform.position, radius);

        foreach (Collider2D collider in colliders)
        {
            Rigidbody2D rb2d = collider.GetComponent<Rigidbody2D>();

            if (rb2d != null)
            {
                float randomHorizontalForce = Random.Range(-1.0f, 1.0f);

                Vector2 force = new Vector2(randomHorizontalForce * horizontalForceMagnitude, verticalForceMagnitude);

                rb2d.AddForce(force, ForceMode2D.Impulse);
            }
        }
    }

}
