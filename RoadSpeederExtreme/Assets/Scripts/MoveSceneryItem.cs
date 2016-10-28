using UnityEngine;
using System.Collections;

public class MoveSceneryItem : MonoBehaviour
{

    [SerializeField]
    float playerSpeed;

    GameController GameControllerScript;


    // Use this for initialization
    void Start()
    {
        GameControllerScript = GameObject.Find("GameScripts").GetComponent<GameController>();
        playerSpeed = GameControllerScript.playerSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        playerSpeed = GameControllerScript.playerSpeed;

        if (this.tag == "Clouds")
        {
            transform.Translate(0.1f, 0, 0.05f, Space.World);
            transform.Translate(0, 0, (playerSpeed / 5 * Time.deltaTime), Space.World);

            if (transform.position.z >= 2900)
            {
                var pos = transform.position;
                pos.z = -3000;
                transform.position = pos;
            }
        }
        else
        {
            transform.Translate(0, 0, playerSpeed * 4 * Time.deltaTime, Space.World);

            if (transform.position.z >= 5100)
                Destroy(this.gameObject);
        }
    }
}
