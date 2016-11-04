import UnityEngine;
import System.Collections;
import System.Collections.Generic;

public class MoveRoadItem extends MonoBehaviour
{

    @SerializeField
    var playerSpeed: float;

    private var GameControllerScript: GameController;
    private var WorldControllerScript: WorldController;
    private var myPool: List.<GameObject>;

    public enum lanes { left, middle, right };
    public var currentLane: lanes;

    // Use this for initialization
    private function Start()
    {
        GameControllerScript = GameObject.Find("GameScripts").GetComponent.<GameController>();
        WorldControllerScript = GameObject.Find("GameScripts").GetComponent.<WorldController>();

        playerSpeed = GameControllerScript.playerSpeed;

        myPool = WorldControllerScript.poolListsContainer[0];

        if (transform.position.x == 0) {
            currentLane = lanes.middle;
        }
        if (transform.position.x < 0) //right
        {
            currentLane = lanes.right;
        }
        if (transform.position.x > 0)
        {
            currentLane = lanes.left;
        }
    }

    // Update is called once per frame
    private function Update()
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
}
