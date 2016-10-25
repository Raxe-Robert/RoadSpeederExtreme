using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

    [SerializeField]
    GameController GameControllerScript;

    public enum lanes { left, middle, right};
    public lanes currentLane;

	// Use this for initialization
	void Start () {
        currentLane = lanes.middle;
        GameControllerScript = GameObject.Find("GameScripts").GetComponent<GameController>();
    }
	
	// Update is called once per frame
	void Update () {
        
        switch(Input.inputString)
        {
            case "a":
                if (currentLane == lanes.middle) {
                    currentLane = lanes.left;
                    transform.position = new Vector3(35, 7.5f, 4970);
                }
                    
                else if (currentLane == lanes.right) {
                    currentLane = lanes.middle;
                    transform.position = new Vector3(0, 7.5f, 4970);
                }
                    
                break;
            case "d":
                if (currentLane == lanes.left) {
                    currentLane = lanes.middle;
                    this.transform.position = new Vector3(0, 7.5f, 4970);
                }
                else if (currentLane == lanes.middle) {
                    currentLane = lanes.right;
                    transform.position = new Vector3(-35, 7.5f, 4970);
                }
                break;
            default:
                break;
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
                Debug.Log("Game over, score: " + GameControllerScript.playerScore);
            }
            //Check for a near miss
            else if (NearMiss(otherScript))
            {
                GameControllerScript.playerScore += 10;
            }
            //If there is one lane between playercar and traffic car
            else
            {
                GameControllerScript.playerScore += 5;
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
