using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public int maxPlayerSpeed;
    public int playerSpeed;
    public float playerScore;

    public int timerSeconds = 0;
    public int timerMinutes = 0;
    
    float unpauseCountdownValue = 3;
    public float unpauseCountdown;

    GameObject pauseMenu;
    GameObject gameTimer;
    GameObject scoreBoard;

    GameObject resumeButton;

    PlayerScript playerScript;
    

    // Use this for initialization
    void Start () {
        maxPlayerSpeed = 400; 
        playerSpeed = 160; 
        playerScore = 0;

        playerScript = GameObject.Find("Playercar").GetComponent<PlayerScript>();

        pauseMenu = GameObject.Find("PauseMenu");
        pauseMenu.SetActive(false);

        gameTimer = GameObject.Find("GameTimer");

        resumeButton = GameObject.Find("UnpauseButton");
        
        scoreBoard = GameObject.Find("HighScoreBoard");
        scoreBoard.SetActive(false);

        StartCoroutine(IncreaseSpeed());
        StartCoroutine(GameTimer());
    }
	
	// Update is called once per frame
	void Update () {
        //Pause or unpause depending on current timeScale
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 1)
            {
                playerScript.enabled = false;
                pauseMenu.SetActive(true);
                Time.timeScale = 0;
            }
            else if (Time.timeScale == 0 && unpauseCountdown <=0)
                StartCoroutine(UnpauseGame());
        } 
         
	}
    
    public void GameOver()
    {
        scoreBoard.SetActive(true);
        Time.timeScale = 0;
        playerScript.enabled = false;
    }
    
    public void RestartGame()
    {
        //Placeholder
        SceneManager.LoadScene("MainScene");
    }

    IEnumerator IncreaseSpeed()
    {
        //Increase speed every x
        while (true)
        {
            if (playerSpeed < maxPlayerSpeed)
            {
                playerSpeed += 2;
            }


            yield return new WaitForSeconds(0.1f * (playerSpeed / 100f));
        }
    }

    IEnumerator GameTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            if (timerSeconds < 59)
            {
                timerSeconds++;
            }
            else
            {
                timerMinutes++;
                timerSeconds = 0;
            }
        }
    }

    IEnumerator UnpauseGame()
    {
        unpauseCountdown = unpauseCountdownValue;

        pauseMenu.SetActive(false);
        gameTimer.SetActive(false);

        while (unpauseCountdown > 0)
        {
            yield return new WaitForSecondsRealtime(1f);
            unpauseCountdown--;
        }
        playerScript.enabled = true;
        Time.timeScale = 1;

        yield return new WaitForSecondsRealtime(0.2f);

        if (unpauseCountdown <= 0)
            gameTimer.SetActive(true);
    }

    public void ButtonFunction_Unpause()
    {
        StartCoroutine(UnpauseGame());
    }

}
