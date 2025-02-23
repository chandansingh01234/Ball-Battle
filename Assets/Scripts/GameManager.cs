using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Android;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Settings")]
    public float matchTime = 140f; // Time limit for each match
    public int totalMatches = 5;  // Total matches in the game

    [Header("UI References")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI PlayerScoreText;
    public TextMeshProUGUI EnemyScoreText;
    public TextMeshProUGUI winLoseText;

    [Header("UI Screens")]
    public GameObject winScreen;
    public GameObject loseScreen;
    public GameObject drawScreen;
    public GameObject penaltyScreen;
    public GameObject playerSwitch;
    public GameObject enemySwitch;
    public GameObject MainMenu;
    public GameObject camera;


    [Header("Dependencies")]
    public EnergyManager energyManager;
    public BallSpawner ballS;


    private float currentTime;
    private bool isMatchActive;
    private int playerScore;
    private int enemyScore;
    private int matchesPlayed;
    private bool isPlayerAttacking;
    public bool isARmode;
    public event Action OnMatchStart;
    public event Action OnMatchEnd;
    public event Action<bool> OnGameEnd; // true for win, false for lose

    private const string CAMERA_PERMISSION = Permission.Camera;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

        }
        else
        {
            Destroy(gameObject);
        }
      
    }

    void Start()
    {
        InitializeGame();
      
    }

    void InitializeGame()
    {
        playerScore = 0;
        enemyScore = 0;
        matchesPlayed = 0;
        UpdateScoreUI();
    }

    void Update()
    {
        


        if (isMatchActive)
        {
            UpdateMatchTimer();
        }
    }
    public void RequestCameraPermission()
    {
        // Check if the camera permission is already granted
        if (!Permission.HasUserAuthorizedPermission(CAMERA_PERMISSION))
        {
            // Request camera permission
            Permission.RequestUserPermission(CAMERA_PERMISSION);

            // Check the permission status after a short delay
            Invoke(nameof(CheckPermissionResult), 1f);
        }
        else
        {
            // Permission already granted, load the AR scene
            LoadARScene();
        }
    }

    private void CheckPermissionResult()
    {
        if (Permission.HasUserAuthorizedPermission(CAMERA_PERMISSION))
        {
            // Permission granted, load the AR scene
            LoadARScene();
        }
        else
        {
            // Permission denied, stay in the main scene
            SceneManager.LoadScene(1);

        }
    }

    private void LoadARScene()
    {
        // Load the AR scene by name or index
        SceneManager.LoadScene(0);
    }
    void UpdateMatchTimer()
    {
        currentTime -= Time.deltaTime;
        UpdateTimerUI();

        if (currentTime <= 0)
        {
            DrawGame();
        }
    }

   public void StartNewMatch()
    {
        
                RotateGameField(180f);


        MainMenu.SetActive(false);
        currentTime = matchTime;
        isMatchActive = true;
        HideAllScreens();
        UpdateTimerUI();
        UpdateScoreUI();
        SwitchRoles();
        OnMatchStart?.Invoke();
    }

    void HideAllScreens()
    {
        winScreen?.SetActive(false);
        loseScreen?.SetActive(false);
        drawScreen?.SetActive(false);
        penaltyScreen?.SetActive(false);
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);
            timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }

    void UpdateScoreUI()
    {
        if (PlayerScoreText != null)
        {
            PlayerScoreText.text = $"{playerScore}";
        }
        if (EnemyScoreText != null)
        {
            EnemyScoreText.text = $"{enemyScore}";
        }
    }

    void SwitchRoles()
    {
        isPlayerAttacking = matchesPlayed % 2 == 0; // Player attacks on even matches, defends on odd matches
        if (isPlayerAttacking)
        {
            

                RotateGameField(0);
            
            // Player attacks, Enemy defends
            playerSwitch.SetActive(true);
            enemySwitch.SetActive(false);
        }
        else
        {
           

                RotateGameField(180f);
            
            // Enemy attacks, Player defends
            playerSwitch.SetActive(false);
            enemySwitch.SetActive(true);

        }
    }

    void RotateGameField(float rotationAngle)
    {
        GameObject targetObject = GameObject.FindGameObjectWithTag("Gamefield");
        if (targetObject != null)
        {
            targetObject.transform.rotation = Quaternion.Euler(0f, rotationAngle, 0f);
        }
    }
    void DestroySoldier()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Attacker");
        if (targets.Length > 0)
        {
            foreach (GameObject target in targets)
            {
                Destroy(target);
            }

        }
        else
        {
            Debug.Log("No GameObjects with tag");
        }
        GameObject[] Dtargets = GameObject.FindGameObjectsWithTag("Defender");
        if (Dtargets.Length > 0)
        {
            foreach (GameObject target in Dtargets)
            {
                Destroy(target);
            }

        }
        else
        {
            Debug.Log("No GameObjects with tag");
        }
        GameObject Btargets = GameObject.FindGameObjectWithTag("Ball");
        if (Btargets!=null)
        {
                Destroy(Btargets);
        }
        else
        {
            Debug.Log("No GameObjects with tag");
        }
    }
    public void WinGame()
    {
        DestroySoldier();
        if(isARmode)
        {
            GameObject.FindGameObjectWithTag("BallS").GetComponent<BallSpawner>().SpawnBall();
            // ARballS.SpawnBall();
        }
        else
        {
            ballS.SpawnBall();

        }
        playerScore++;
        matchesPlayed++;
        EndMatch(true);
        ShowEndScreen(winScreen, "You Win!");
        CheckForGameEnd();
    }

    public void LoseGame()
    {
        DestroySoldier();
        if (isARmode)
        {
            GameObject.FindGameObjectWithTag("BallS").GetComponent<BallSpawner>().SpawnBall();
            //ARballS.SpawnBall();
        }
        else
        {
            ballS.SpawnBall();

        }
        enemyScore++;
        matchesPlayed++;
        EndMatch(false);
        ShowEndScreen(loseScreen, "You Lose!");
        CheckForGameEnd();
    }

    public void DrawGame()
    {
        DestroySoldier();
        if (isARmode)
        {
            GameObject.FindGameObjectWithTag("Balls").GetComponent<BallSpawner>().SpawnBall();
            // ARballS.SpawnBall();
        }
        else
        {
            ballS.SpawnBall();

        }
        matchesPlayed++;
        EndMatch(false);
        ShowEndScreen(drawScreen, "Draw!");
        CheckForGameEnd();
    }

    void ShowEndScreen(GameObject screen, string message)
    {
        HideAllScreens();
        screen?.SetActive(true);
        if (winLoseText != null)
        {
            winLoseText.text = message;
        }
        UpdateScoreUI();
    }

    void EndMatch(bool isWin)
    {
        isMatchActive = false;
        OnMatchEnd?.Invoke();
    }

    void CheckForGameEnd()
    {
        if (matchesPlayed >= totalMatches)
        {
            HandleGameEnd();
        }
        else
        {
            Invoke(nameof(StartNewMatch), 3f); // Start next match after 3 seconds
        }
    }

    void HandleGameEnd()
    {
        if (playerScore > enemyScore)
        {
            ShowEndScreen(winScreen, "You Win the Game!");
            OnGameEnd?.Invoke(true);
        }
        else if (enemyScore > playerScore)
        {
            ShowEndScreen(loseScreen, "You Lose the Game!");
            OnGameEnd?.Invoke(false);
        }
        else
        {
            StartPenaltyGame();
        }
    }
 
    public void OpenARScene()
    {
        camera.SetActive(false);

        SceneManager.LoadScene(0);
    }
    void StartPenaltyGame()
    {

        HideAllScreens();
        penaltyScreen?.SetActive(true);
        if (winLoseText != null)
        {
            winLoseText.text = "Penalty Game!";
        }
    }
    public void LoadPanalityGame()
    {
        SceneManager.LoadScene("Panality Game");
    }
    public void OpenMenu()
    {
        MainMenu.SetActive(true);
    }
    public void CancelMenu()
    {
        MainMenu.SetActive(false);
    }
 
    public void QuitGame()
    {
        Application.Quit();
    }
    public void GameOver()
    {
        Debug.Log("Game Over: Time's up!");
        // Handle game over logic (e.g., show game over screen)
    }

    public bool IsMatchActive()
    {
        return isMatchActive;
    }

    public float GetRemainingTime()
    {
        return currentTime;
    }

    public (int playerScore, int enemyScore) GetCurrentScore()
    {
        return (playerScore, enemyScore);
    }

}