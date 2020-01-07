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
        switch (this.tag)
        {
            case "Traffic":
                myPool = WorldControllerScript.poolListsContainer[0];
                break;
            case "Stepperollers":
                myPool = WorldControllerScript.poolListsContainer[9];
                break;
            default:
                break;
        }
        //myPool = WorldControllerScript.poolListsContainer[0];
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
        else if (this.tag == "Stepperollers")
        {
            transform.Translate(0.05f, 0, 0.006f, Space.World);
            transform.Translate(0, 0, (playerSpeed / 4 * Time.deltaTime), Space.World);
            transform.Rotate(-1f, 0, -5f);

            //Between -5 and -2.2 = right
            if (transform.position.x < -5 && transform.position.x > -2.2)
                currentLane = roadLanes.right;
            //Between -2.2 and 2.2 = middle;
            else if (transform.position.x > -2.2 && transform.position.x < 2.2)
                currentLane = roadLanes.middle;
            //Between 2.2 and 5 = left;
            else if (transform.position.x > 2.2 && transform.position.x < 5)
                currentLane = roadLanes.left;


            if (transform.position.z >= 560)
            {
                gameObject.SetActive(false);
                if (myPool != null)
                    myPool.Add(gameObject);
            }
        }
    }

