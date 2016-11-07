import System.Collections.Generic;
    @SerializeField
    var playerSpeed: float;

    private var GameControllerScript: GameController;
    private var WorldControllerScript: WorldController;
    private var myPool: List.<GameObject>;

    public enum roadLanes { left, middle, right };
    public var currentLane: roadLanes;

    function OnEnable()
    {
        if (transform.position.x == 0)
        {
            currentLane = roadLanes.middle;
        }
        if (transform.position.x < 0)
        {
            currentLane = roadLanes.right;
        }
        if (transform.position.x > 0)
        {
            currentLane = roadLanes.left;
        }
    }

    // Use this for initialization
    function Start()
    {
        GameControllerScript = GameObject.Find("GameScripts").GetComponent.<GameController>();
        WorldControllerScript = GameObject.Find("GameScripts").GetComponent.<WorldController>();

        playerSpeed = GameControllerScript.playerSpeed;

        myPool = WorldControllerScript.poolListsContainer[0];
    }

    // Update is called once per frame
    function Update()
    {
        playerSpeed = GameControllerScript.playerSpeed;
        
        if (this.tag == "Traffic")
        {

            transform.Translate(0, 0, (playerSpeed - 120) / 2 * Time.deltaTime, Space.World);

            if (transform.position.z >= 510)
            {
                gameObject.SetActive(false);
                myPool.Add(gameObject);
            }
        }
    }

