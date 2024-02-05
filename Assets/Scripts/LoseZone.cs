using UnityEngine;

public class LoseZone : MonoBehaviour
{
    [SerializeField] private GameObject _loseTab;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ball"))
        {
            _loseTab.SetActive(true);
        }
    }
}
