using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BallsSpawner))]
public class PlayerBallRandomChoice : MonoBehaviour
{
    [SerializeField] private Image[] _ballsImages = new Image[6];
    [SerializeField] private int _playerBallsCount = 6; // Первые N шаров, которые может получить игрок.
    [SerializeField] private Vector3 _spawnPos = new Vector3(0, 3.5f, 0);

    private BallsSpawner _ballsSpawner;
    private int _nextBall = 0;

    public Vector3 SpawnPos { get => _spawnPos; set => _spawnPos = value; }

    private void Awake()
    {
        _ballsSpawner = GetComponent<BallsSpawner>();
    }

    public GameObject NextBall()
    {
        int randomBall = _nextBall;
        _nextBall = Random.Range(0, _playerBallsCount);

        for (int i = 0; i < _ballsImages.Length; i++)
        {
            if (i == _nextBall)
            {
                _ballsImages[i].gameObject.SetActive(true);
            }
            else
            {
                _ballsImages[i].gameObject.SetActive(false);
            }
        }

        GameObject ball = _ballsSpawner.SpawnBall(_spawnPos, _ballsSpawner.BallsPrefabs[randomBall]);
        return ball;
    }
}
