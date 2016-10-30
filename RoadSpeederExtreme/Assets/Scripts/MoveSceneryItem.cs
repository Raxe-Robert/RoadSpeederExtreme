using UnityEngine;
using System.Collections;

public class MoveSceneryItem : MonoBehaviour
{

    [SerializeField]
    float playerSpeed;

    bool disableAllOtherTerrains;

    GameController GameControllerScript;
    WorldController WorldControllerScript;

    void OnEnable()
    {
        disableAllOtherTerrains = true;
    }

    // Use this for initialization
    void Start()
    {
        GameControllerScript = GameObject.Find("GameScripts").GetComponent<GameController>();
        WorldControllerScript = GameObject.Find("GameScripts").GetComponent<WorldController>();
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
        else if (this.tag == "Terrain")
        {
            if (transform.position.z <= 1000)
                transform.Translate(0, 0, playerSpeed * 4 * Time.deltaTime, Space.World);

            if (transform.position.z > 1000 && disableAllOtherTerrains == true)
            {
                foreach (var terrain in WorldControllerScript.LandscapeTerrain)
                {
                    if (terrain.name != this.name && terrain.transform.position.z > 1000)
                    {
                        var posOther = terrain.transform.position;
                        posOther.z = -5000;
                        terrain.transform.position = posOther;

                        terrain.SetActive(false);
                    }
                }

                var pos = transform.position;
                pos.y = -0.5f;
                transform.position = pos;

                disableAllOtherTerrains = false;
            }
        }
        else if (this.tag == "Bridges")
        {
            transform.Translate(0, 0, playerSpeed * 4 * Time.deltaTime, Space.World);

            if (transform.position.z >= 5600)
                Destroy(this.gameObject);
        }
        else
        {
            transform.Translate(0, 0, playerSpeed * 4 * Time.deltaTime, Space.World);

            if (transform.position.z >= 5100)
                Destroy(this.gameObject);
        }
    }
}
