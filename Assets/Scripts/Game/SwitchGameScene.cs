using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchGameScene : MonoBehaviour
{
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OpenMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
