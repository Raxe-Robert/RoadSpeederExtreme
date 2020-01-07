
import UnityEngine.UI;

	@SerializeField
	var GameControllerScript: GameController;
	private var UIhandlerScript: UIhandler;
    
    public enum lanes { left, middle, right};
	public var currentLane: lanes;

	// Use this for initialization
	function Start() {
        currentLane = lanes.middle;
		GameControllerScript = GameObject.Find("GameScripts").GetComponent.<GameController>();
		UIhandlerScript = GameObject.Find("UI").GetComponent.<UIhandler>();
    }
	
	// Update is called once per frame
    function Update() {

        if (Input.touchCount > 0) {
            var touch = Input.GetTouch(0);
            if (touch.position.x < Screen.width / 2) {
                if (currentLane == lanes.middle) {
                    currentLane = lanes.left;
                    transform.position = new Vector3(3.5, 0.75, 497);
                }

                else if (currentLane == lanes.right) {
                    currentLane = lanes.middle;
                    transform.position = new Vector3(0, 0.75, 497);
                }
            }
            else if (touch.position.x >= Screen.width / 2) {
                if (currentLane == lanes.left) {
                    currentLane = lanes.middle;
                    this.transform.position = new Vector3(0, 0.75, 497);
                }
                else if (currentLane == lanes.middle) {
                    currentLane = lanes.right;
                    transform.position = new Vector3(-3.5, 0.75, 497);
                }
            }
        }

		
		else if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
		{
			if (currentLane == lanes.middle)
			{
				currentLane = lanes.left;
				transform.position = new Vector3(3.5, 0.75, 497);
			}

			else if (currentLane == lanes.right)
			{
				currentLane = lanes.middle;
				transform.position = new Vector3(0, 0.75, 497);
			}
		}
		else if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
		{
			if (currentLane == lanes.left)
			{
				currentLane = lanes.middle;
				this.transform.position = new Vector3(0, 0.75, 497);
			}
			else if (currentLane == lanes.middle)
			{
				currentLane = lanes.right;
				transform.position = new Vector3(-3.5, 0.75, 497);
			}
		}
	}

	function OnTriggerEnter(other: Collider)
	{
		var otherScript: MoveRoadItem = other.gameObject.GetComponent.<MoveRoadItem>();
		if (other.gameObject.tag == "Traffic")
		{
			//If the lanes are the same there is a collision
			if (currentLane.ToString() == otherScript.currentLane.ToString())
            {
                GameControllerScript.GameOver();                              
			}
			//Check for a near miss
			else if (NearMiss(otherScript))
			{
                var scoreToAdd: int = 10 + (60 * GameControllerScript.timerMinutes) + (10 * (GameControllerScript.timerSeconds / 10));

                GameControllerScript.playerScore += scoreToAdd;
                UIhandlerScript.NewMessage(scoreToAdd.ToString());
			}
			//If there is one lane between playercar and traffic car
			else
			{
                scoreToAdd = 5 + (30 * GameControllerScript.timerMinutes) + (5 * (GameControllerScript.timerSeconds / 10));

                GameControllerScript.playerScore += scoreToAdd;
				UIhandlerScript.NewMessage(scoreToAdd.ToString());
			}	
		}
		if (other.gameObject.tag == "Stepperollers")
        {
            if (currentLane.ToString() == otherScript.currentLane.ToString()) {
                var scoreToAddBonus: int = 1000;
                GameControllerScript.playerScore += scoreToAddBonus;
                UIhandlerScript.NewMessage(scoreToAddBonus.ToString());
            }
        }
	}

	function NearMiss(otherScript: MoveRoadItem): boolean
	{
		if (currentLane == lanes.middle)
		{
			return true;
		}
		else if (otherScript.currentLane.ToString() == "middle")
		{
			return true;
		}

		return false;
	}

