using UnityEngine;
using System.Collections;

public class MoveSceneryItem : MonoBehaviour
{

    [SerializeField]
    float playerSpeed;

    bool playSpawnAnimation;

    GameController GameControllerScript;


    // Use this for initialization
    void Start()
    {
        GameControllerScript = GameObject.Find("GameScripts").GetComponent<GameController>();
        playerSpeed = GameControllerScript.playerSpeed;

        if (this.tag == "Clouds")
            playSpawnAnimation = false;
        else
            playSpawnAnimation = false; //set to true
    }

    // Update is called once per frame
    void Update()
    {
        playerSpeed = GameControllerScript.playerSpeed;


        if (this.tag == "Trees" || this.tag == "Bushes")
        {
            transform.Translate(0, 0, playerSpeed * 4 * Time.deltaTime);

            if (transform.position.z >= 5100)
                Destroy(this.gameObject);
        }

        if (this.tag == "Clouds")
        {
            transform.Translate(0.1f, 0, 0.05f);
            transform.Translate(0, 0, (playerSpeed / 5 * Time.deltaTime));

            if (transform.position.z >= 5100)
            {
                var pos = transform.position;
                pos.z = 1500;
                transform.position = pos;
            }
        }

        if (this.tag == "Buildings")
        {
            transform.Translate(0, 0, playerSpeed * 4 * Time.deltaTime);

            if (transform.position.z >= 5100)
                Destroy(this.gameObject);
        }
    }
}
