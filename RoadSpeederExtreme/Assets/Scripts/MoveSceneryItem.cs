using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveSceneryItem : MonoBehaviour
{

    [SerializeField]
    float playerSpeed;

    bool disableAllOtherTerrains;

    GameController GameControllerScript;
    WorldController WorldControllerScript;
    List<GameObject> myPool;

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
        
        switch (this.tag)
        {
            case "Clouds":
                myPool = WorldControllerScript.poolListsContainer[1];
                transform.position = new Vector3(Random.Range(-4000, 4000), Random.Range(680, 700), Random.Range(-3000, 2800));
                break;
            case "Trees":
                myPool = WorldControllerScript.poolListsContainer[2];
                break;
            case "Bushes":
                myPool = WorldControllerScript.poolListsContainer[3];
                break;
            case "Buildings":
                myPool = WorldControllerScript.poolListsContainer[4];
                break;
            case "Cactuses":
                myPool = WorldControllerScript.poolListsContainer[5];
                break;
            case "DesertFormations":
                myPool = WorldControllerScript.poolListsContainer[6];
                break;
            case "Bridges":
                myPool = WorldControllerScript.poolListsContainer[7];
                break;
            case "Waves":
                myPool = WorldControllerScript.poolListsContainer[8];
                break;
            default:
                myPool = null;
                break;
        }
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
            {
                gameObject.SetActive(false);
                if (myPool != null)
                    myPool.Add(gameObject);
            }
        }
        else
        {
            transform.Translate(0, 0, playerSpeed * 4 * Time.deltaTime, Space.World);

            if (transform.position.z >= 5100)
            {
                gameObject.SetActive(false);
                if (myPool != null)
                    myPool.Add(gameObject);
            }
        }
    }
}
