using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _recordScoreText;
    [SerializeField] private TextMeshProUGUI _settingsRecordScoreText;
    [SerializeField] private TextMeshProUGUI _newRecordText;

    private int _playerScore = 0;

    private void Start()
    {
        _settingsRecordScoreText.text = $"Ваш рекорд: {PlayerPrefs.GetInt("Record")}";
    }

    private void OnEnable()
    {
        LoseZone.OnLose += UpdateRecordScore;
    }

    private void OnDisable()
    {
        LoseZone.OnLose -= UpdateRecordScore;
    }

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

    private void UpdateRecordScore()
    {
        if (PlayerPrefs.HasKey("Record"))
        {
            if (_playerScore > PlayerPrefs.GetInt("Record"))
            {
                PlayerPrefs.SetInt("Record", _playerScore);
                _newRecordText.gameObject.SetActive(true);
            }
        }
        else if (!PlayerPrefs.HasKey("Record"))
        {
            PlayerPrefs.SetInt("Record", _playerScore);
            _newRecordText.gameObject.SetActive(true);
        }

        _recordScoreText.text = $"Ваш рекорд: {PlayerPrefs.GetInt("Record")}";
        _settingsRecordScoreText.text = $"Ваш рекорд: {PlayerPrefs.GetInt("Record")}";
    }
}
