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

        if (playSpawnAnimation == true)
        {
            if (this.tag == "Trees")
            {
                transform.Translate(0, -2000 * Time.deltaTime, 0);

                if (transform.position.y <= 45)
                {
                    var pos = transform.position;
                    pos.y = Random.Range(30, 40);
                    transform.position = pos;
                    playSpawnAnimation = false;

                }
            }
            if (this.tag == "Bushes")
            {
                transform.Translate(0, -2000 * Time.deltaTime, 0);

                if (transform.position.y <= 8.7f)
                {
                    var pos = transform.position;
                    pos.y = 8.7f;
                    transform.position = pos;
                    playSpawnAnimation = false;

                }
            }
        }
        else
        {
            if (this.tag == "Trees" || this.tag == "Bushes")
            {
                transform.Translate(0, 0, playerSpeed * 4 * Time.deltaTime);
            }

            if (this.tag == "Clouds")
            {
                transform.Translate(0.1f, 0, 0.05f);
                transform.Translate(0, 0, (playerSpeed / 5 * Time.deltaTime));
            }
        }
       

        if (transform.position.z >= 5100)
            Destroy(this.gameObject);

    }
}
