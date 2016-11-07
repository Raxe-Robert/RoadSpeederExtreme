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
                transform.position = new Vector3(Random.Range(-400, 400), Random.Range(68, 70), Random.Range(-300, 280));
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
            transform.Translate(0.01f, 0, 0.005f, Space.World);
            transform.Translate(0, 0, (playerSpeed / 4 * Time.deltaTime), Space.World);

            if (transform.position.z >= 290)
            {
                var pos = transform.position;
                pos.z = -300;
                transform.position = pos;
            }
        }
        else if (this.tag == "Terrain")
        {
            if (transform.position.z <= 100)
                transform.Translate(0, 0, playerSpeed * Time.deltaTime, Space.World);

            if (transform.position.z > 100 && disableAllOtherTerrains == true)
            {
                foreach (var terrain in WorldControllerScript.LandscapeTerrain)
                {
                    if (terrain.name != this.name && terrain.transform.position.z > 100)
                    {
                        var posOther = terrain.transform.position;
                        posOther.z = -500;
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
            transform.Translate(0, 0, playerSpeed * Time.deltaTime, Space.World);

            if (transform.position.z >= 560)
            {
                gameObject.SetActive(false);
                if (myPool != null)
                    myPool.Add(gameObject);
            }
        }
        else
        {
            transform.Translate(0, 0, playerSpeed * Time.deltaTime, Space.World);

            if (transform.position.z >= 510)
            {
                gameObject.SetActive(false);
                if (myPool != null)
                    myPool.Add(gameObject);
            }
        }
    }
}
