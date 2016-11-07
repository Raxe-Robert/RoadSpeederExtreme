
import UnityEngine.SceneManagement;

    public var maxPlayerSpeed: int;
    public var playerSpeed: int;
    public var playerScore: float;

    public var timerSeconds: int = 0;
    public var timerMinutes: int = 0;
    
    private var unpauseCountdownValue: float = 3;
    public var unpauseCountdown: float;

    private var pauseMenu: GameObject;
    private var gameTimer: GameObject;
    private var scoreBoard: GameObject;

    private var resumeButton: GameObject;

    private var playerScript: PlayerScript;
    

    // Use this for initialization
    function Start() {
        maxPlayerSpeed = 400; 
        playerSpeed = 160; 
        playerScore = 0;

        playerScript = GameObject.Find("Playercar").GetComponent.<PlayerScript>();

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
	function Update() {
        //Pause or unpause depending on current timeScale
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 1)
            {
                playerScript.enabled = false;
                pauseMenu.SetActive(true);
                Time.timeScale = 0;
            }
            else if (Time.timeScale == 0 && unpauseCountdown <=0 && scoreBoard.gameObject.activeSelf == false)
                StartCoroutine(UnpauseGame());
        } 
         
	}
    
    public function GameOver()
    {
        scoreBoard.SetActive(true);
        Time.timeScale = 0;
        playerScript.enabled = false;
    }
    
    public function RestartGame()
    {
        //Placeholder
        SceneManager.LoadScene("MainSceneJS");
        Time.timeScale = 1;
    }

    private function IncreaseSpeed(): IEnumerator
    {
        //Increase speed every x
        while (true)
        {
            if (playerSpeed < maxPlayerSpeed)
            {
                playerSpeed += 2;
            }


            yield WaitForSeconds(0.1 * (playerSpeed / 100));
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

    function UnpauseGame(): IEnumerator
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

    public function ButtonFunction_Unpause()
    {
        StartCoroutine(UnpauseGame());
    }


