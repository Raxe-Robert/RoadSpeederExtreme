using UnityEngine;
using System.Collections;

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
    PlayerScript playerScript;
    GameObject resumeButton;
    

    // Use this for initialization
    void Start () {
        maxPlayerSpeed = 600;
        playerSpeed = 130;
        playerScore = 0;

        playerScript = GameObject.Find("Playercar").GetComponent<PlayerScript>();

        pauseMenu = GameObject.Find("PauseMenu");
        pauseMenu.SetActive(false);

        gameTimer = GameObject.Find("GameTimer");
        gameTimer.SetActive(true);

        resumeButton = GameObject.Find("UnpauseButton");

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
    
    IEnumerator IncreaseSpeed()
    {
        //Increase speed every x
        while (true)
        {
            playerSpeed++;
            
            yield return new WaitForSeconds(1f);
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

        while (unpauseCountdown > 0)
        {
            gameTimer.SetActive(false);
            yield return new WaitForSecondsRealtime(1f);
            unpauseCountdown--;
        }
        playerScript.enabled = true;
        gameTimer.SetActive(true);
        Time.timeScale = 1;
    }
}
