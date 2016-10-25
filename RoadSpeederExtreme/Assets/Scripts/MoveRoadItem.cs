using UnityEngine;
using System.Collections;

public class MoveRoadItem : MonoBehaviour
{

    [SerializeField]
    float playerSpeed;

    GameController GameControllerScript;

    public enum lanes { left, middle, right };
    public lanes currentLane;

    // Use this for initialization
    void Start()
    {
        GameControllerScript = GameObject.Find("GameScripts").GetComponent<GameController>();
        playerSpeed = GameControllerScript.playerSpeed;

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
            transform.Translate(playerSpeed * 2 * Time.deltaTime - 120 * Time.deltaTime, 0, 0);
        }

        if (transform.position.z >= 5100)
            Destroy(this.gameObject);

    }
}
