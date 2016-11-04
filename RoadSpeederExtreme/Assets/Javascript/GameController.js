import UnityEngine;
import System.Collections;

public class GameController extends MonoBehaviour {

    public var maxPlayerSpeed: int;
    public var playerSpeed: int;
    public var playerScore: float;

    public var timerSeconds: int = 0;
    public var timerMinutes: int = 0;
    
    private var unpauseCountdownValue: float = 3;
    public var unpauseCountdown: float;

    private var pauseMenu: GameObject;
    private var gameTimer: GameObject;
    private var resumeButton: GameObject;

    private var playerScript: PlayerScript;
    

    // Use this for initialization
    private function Start() {
        maxPlayerSpeed = 400; 
        playerSpeed = 160; 
        playerScore = 0;

        playerScript = GameObject.Find("Playercar").GetComponent.<PlayerScript>();

        pauseMenu = GameObject.Find("PauseMenu");
        pauseMenu.SetActive(false);

        gameTimer = GameObject.Find("GameTimer");

        resumeButton = GameObject.Find("UnpauseButton");

        StartCoroutine(IncreaseSpeed());
        StartCoroutine(GameTimer());
    }
	
	// Update is called once per frame
	private function Update() {
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
    
    private function IncreaseSpeed(): IEnumerator
    {
        //Increase speed every x
        while (true)
        {
            if (playerSpeed < maxPlayerSpeed)
            {
                Debug.Log(playerSpeed);
                playerSpeed += 2;
            }


            yield  WaitForSeconds(0.1 * (playerSpeed / 100));
        }
    }

    private function GameTimer(): IEnumerator
    {
        while (true)
        {
            yield  WaitForSeconds(1);

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

    private function UnpauseGame(): IEnumerator
    {
        unpauseCountdown = unpauseCountdownValue;

        pauseMenu.SetActive(false);
        gameTimer.SetActive(false);

        while (unpauseCountdown > 0)
        {
            yield  WaitForSecondsRealtime(1);
            unpauseCountdown--;
        }
        playerScript.enabled = true;
        Time.timeScale = 1;

        yield  WaitForSecondsRealtime(0.2);

        if (unpauseCountdown <= 0)
            gameTimer.SetActive(true);
    }
}
