using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerScore))]
public class BallsSpawner : MonoBehaviour
{
    public static BallsSpawner Instance;

    [SerializeField] private GameObject[] _ballsPrefabs = new GameObject[12];
    [SerializeField] private AudioClip _popSound;

    private AudioSource _audioSource;
    private PlayerScore _playerScore;

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
        _playerScore = GetComponent<PlayerScore>();
    }

    public void SpawnCombinedBall(Vector3 spawnPos, GameObject identityBallsObject)
    {
        GameObject ballForSpawn = null;
        bool isLast = false;


        for (int i = 0; i < _ballsPrefabs.Length; i++)
        {
            if (identityBallsObject.GetComponent<SpriteRenderer>().sprite == _ballsPrefabs[i].GetComponent<SpriteRenderer>().sprite)
            {
                if (i + 1 < _ballsPrefabs.Length)
                {
                    ballForSpawn = _ballsPrefabs[i + 1];
                    break;
                }

                isLast = true;
            }
        }

        if (ballForSpawn != null || isLast == true)
        {
            GameObject ball = null;
            ParticleSystem particles = null;

            if (isLast == true)
            {
                particles = identityBallsObject.GetComponent<IdenticalBallsTouch>().PopParticles;
                particles.transform.parent = null;
                particles.gameObject.SetActive(true);

                _playerScore.AddScore(12);
            }
            else if (isLast == false)
            {
                ball = Instantiate(ballForSpawn, spawnPos, identityBallsObject.transform.rotation);
                particles = ball.GetComponent<IdenticalBallsTouch>().PopParticles;
                particles.gameObject.SetActive(true);

                _playerScore.AddScore(Array.IndexOf(_ballsPrefabs, ballForSpawn));
            }

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