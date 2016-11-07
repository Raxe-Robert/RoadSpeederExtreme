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
	}

	void OnTriggerEnter(Collider other)
	{
		MoveRoadItem otherScript = other.gameObject.GetComponent<MoveRoadItem>();
        MoveSceneryItem sceneryScript = other.gameObject.GetComponent<MoveSceneryItem>();
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
                int scoreToAdd = 10 + (60 * GameControllerScript.timerMinutes) + (10 * (GameControllerScript.timerSeconds / 10));

                GameControllerScript.playerScore += scoreToAdd;
                UIhandlerScript.NewMessage(scoreToAdd.ToString());
			}
			//If there is one lane between playercar and traffic car
			else
			{
                int scoreToAdd = 5 + (30 * GameControllerScript.timerMinutes) + (5 * (GameControllerScript.timerSeconds / 10));

                GameControllerScript.playerScore += scoreToAdd;
				UIhandlerScript.NewMessage(scoreToAdd.ToString());
			}	
		}
        if (other.gameObject.tag == "Stepperollers")
        {
            int scoreToAdd = 1000;
            Destroy(other.gameObject);
            GameControllerScript.playerScore += scoreToAdd;
            UIhandlerScript.NewMessage(scoreToAdd.ToString());
        }
	}

	bool NearMiss(MoveRoadItem otherScript)
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
}
