using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveRoadItem : MonoBehaviour
{

    [SerializeField]
    float playerSpeed;

    GameController GameControllerScript;
    WorldController WorldControllerScript;
    List<GameObject> myPool;

    public enum lanes { left, middle, right };
    public lanes currentLane;

    // Use this for initialization
    void Start()
    {
        GameControllerScript = GameObject.Find("GameScripts").GetComponent<GameController>();
        WorldControllerScript = GameObject.Find("GameScripts").GetComponent<WorldController>();

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
    void Update()
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
