using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

    enum lanes { left, middle, right};
    lanes currentLane;

	// Use this for initialization
	void Start () {
        currentLane = lanes.middle;

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
}
