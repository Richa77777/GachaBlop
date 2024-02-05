using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;

    private int _playerScore = 0;

    public void AddScore(int score)
    {
        if (score > 0)
        {
            _playerScore += score;
            UpdateScore();
        }
    }

    private void UpdateScore()
    {
        _scoreText.text = _playerScore.ToString();
    }
}
