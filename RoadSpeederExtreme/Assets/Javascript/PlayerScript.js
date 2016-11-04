import UnityEngine;
import System.Collections;

public class PlayerScript extends MonoBehaviour {

	@SerializeField
	var GameControllerScript: GameController;
	private var UIhandlerScript: UIhandler;

	public enum lanes { left, middle, right};
	public var currentLane: lanes;

	// Use this for initialization
	private function Start() {
		currentLane = lanes.middle;
		GameControllerScript = GameObject.Find("GameScripts").GetComponent.<GameController>();
		UIhandlerScript = GameObject.Find("UI").GetComponent.<UIhandler>();
	}
	
	// Update is called once per frame
	private function Update() {
		
		if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
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
		if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
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
		if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
		{
			GameControllerScript.playerSpeed += 100;
		}
		if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
		{
			GameControllerScript.playerSpeed -= 100;
		}
	}

	private function OnTriggerEnter(other: Collider)
	{
		var otherScript: MoveRoadItem = other.gameObject.GetComponent.<MoveRoadItem>();
		if (other.gameObject.tag == "Traffic")
		{
			//If the lanes are the same there is a collision
			if (currentLane.ToString() == otherScript.currentLane.ToString())
			{
				Debug.Log("Game over, score: " + GameControllerScript.playerScore);
			}
			//Check for a near miss
			else if (NearMiss(otherScript))
			{
				GameControllerScript.playerScore += 10;
				UIhandlerScript.NewMessage(10.ToString());
			}
			//If there is one lane between playercar and traffic car
			else
			{
				GameControllerScript.playerScore += 5;
				UIhandlerScript.NewMessage(5.ToString());
			}
				
		}
	}

	private function NearMiss(otherScript: MoveRoadItem): boolean
	{
		if (currentLane.ToString() == "middle")
		{
			return true;
		}
		else if (otherScript.currentLane.ToString() == "middle")
		{
			return true;
		}

		return false;
	}
}
