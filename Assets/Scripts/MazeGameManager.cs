using UnityEngine;

public class MazeGameManager : MonoBehaviour
{
    public static MazeGameManager Instance;
    public GameObject Win_message;
    public GameObject Gameover_message;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayerWins()
    {
        Win_message.SetActive(true);
        Time.timeScale = 0f;
        // Handle win logic
    }

    public void GameOver()
    {
        Gameover_message.SetActive(true);
        Time.timeScale = 0f;
        // Handle game over logic
    }
    public void QuitGame()
    {
        Application.Quit();
    }

} 