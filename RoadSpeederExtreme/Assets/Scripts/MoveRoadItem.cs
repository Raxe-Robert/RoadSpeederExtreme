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

    void OnEnable()
    {
        if (transform.position.x == 0)
        {
            currentLane = lanes.middle;
        }
        if (transform.position.x < 0)
        {
            currentLane = lanes.right;
        }
        if (transform.position.x > 0)
        {
            currentLane = lanes.left;
        }
    }

    // Use this for initialization
    void Start()
    {
        GameControllerScript = GameObject.Find("GameScripts").GetComponent<GameController>();
        WorldControllerScript = GameObject.Find("GameScripts").GetComponent<WorldController>();

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
                if (myPool != null)
                    myPool.Add(gameObject);
            }
        }
        else if (this.tag == "Stepperollers")
        {
            transform.Translate(0.05f, 0, 0.006f, Space.World);
            transform.Translate(0, 0, (playerSpeed / 4 * Time.deltaTime), Space.World);
            transform.Rotate(-1f, 0, -5f);

            if (transform.position.z >= 560)
            {
                gameObject.SetActive(false);
                if (myPool != null)
                    myPool.Add(gameObject);
            }
        }
    }
}
