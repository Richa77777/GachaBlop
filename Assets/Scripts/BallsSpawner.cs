using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallsSpawner : MonoBehaviour
{
    public static BallsSpawner Instance;

    [SerializeField] private GameObject[] _ballsPrefabs = new GameObject[12];
    [SerializeField] private AudioClip _popSound;

    private AudioSource _audioSource;

    public GameObject[] BallsPrefabs => _ballsPrefabs;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        _audioSource = GetComponent<AudioSource>();
    }

    public void SpawnCombinedBall(Vector3 spawnPos, GameObject identityBallsObject)
    {
        GameObject ballForSpawn = null;

        for (int i = 0; i < _ballsPrefabs.Length; i++)
        {
            if (identityBallsObject.GetComponent<SpriteRenderer>().sprite == _ballsPrefabs[i].GetComponent<SpriteRenderer>().sprite)
            {
                if (i + 1 < _ballsPrefabs.Length)
                {
                    ballForSpawn = _ballsPrefabs[i + 1];
                    break;
                }

            }
        }

        if (ballForSpawn != null)
        {
            GameObject ball = Instantiate(ballForSpawn, spawnPos, identityBallsObject.transform.rotation);

            ball.GetComponent<IdenticalBallsTouch>().PopParticles.gameObject.SetActive(true);
            _audioSource.PlayOneShot(_popSound);
        }
    }

    public GameObject SpawnBall(Vector3 spawnPos, GameObject ballObject)
    {
        GameObject ball = null;

        if (ballObject != null)
        {
            ball = Instantiate(ballObject, spawnPos, Quaternion.identity);
            StartCoroutine(SmoothAppearance(ball, 0.2f));
        }


        return ball;
    }

    private IEnumerator SmoothAppearance(GameObject obj, float duration)
    {
        Vector3 startScale = Vector3.zero;
        Vector3 targetScale = new Vector3(obj.transform.localScale.x + 0.1f, obj.transform.localScale.y + 0.1f, obj.transform.localScale.z + 0.1f);

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            obj.transform.localScale = Vector3.Lerp(startScale, targetScale, t / duration);
            yield return null;
        }

        obj.transform.localScale = targetScale;

        startScale = obj.transform.localScale;
        targetScale = new Vector3(startScale.x - 0.1f, startScale.y - 0.1f, startScale.z - 0.1f);

        //yield return new WaitForSeconds(0.05f);

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            obj.transform.localScale = Vector3.Lerp(startScale, targetScale, t / duration);
            yield return null;
        }

        obj.transform.localScale = targetScale;
    }
}