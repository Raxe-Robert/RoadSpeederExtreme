using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {

	[SerializeField]
	GameController GameControllerScript;
	UIhandler UIhandlerScript;
    
    public enum lanes { left, middle, right};
	public lanes currentLane;

	// Use this for initialization
	void Start () {
        currentLane = lanes.middle;
		GameControllerScript = GameObject.Find("GameScripts").GetComponent<GameController>();
		UIhandlerScript = GameObject.Find("UI").GetComponent<UIhandler>();
    }
	
	// Update is called once per frame
	void Update () {
		
		if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
		{
			if (currentLane == lanes.middle)
			{
				currentLane = lanes.left;
				transform.position = new Vector3(3.5f, 0.75f, 497);
			}

			else if (currentLane == lanes.right)
			{
				currentLane = lanes.middle;
				transform.position = new Vector3(0, 0.75f, 497);
			}
		}
		if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
		{
			if (currentLane == lanes.left)
			{
				currentLane = lanes.middle;
				this.transform.position = new Vector3(0, 0.75f, 497);
			}
			else if (currentLane == lanes.middle)
			{
				currentLane = lanes.right;
				transform.position = new Vector3(-3.5f, 0.75f, 497);
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

	void OnTriggerEnter(Collider other)
	{
		MoveRoadItem otherScript = other.gameObject.GetComponent<MoveRoadItem>();
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

	bool NearMiss(MoveRoadItem otherScript)
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
