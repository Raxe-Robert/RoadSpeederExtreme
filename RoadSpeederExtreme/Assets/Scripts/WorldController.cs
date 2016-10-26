using UnityEngine;
using System.Collections;

public class WorldController : MonoBehaviour
{

    [SerializeField]
    GameObject[] spawnableObjects;

    GameController GameControllerScript;

    [SerializeField]
    int[] spawnzoneTrees;
    int objectSpawnHeight;

    float playerSpeed;
    float spawnrate = 0; //seconds

    Vector3 tempSpawnPosition;

    // Use this for initialization
    void Start()
    {
        GameControllerScript = GameObject.Find("GameScripts").GetComponent<GameController>();
        playerSpeed = GameControllerScript.playerSpeed;
        objectSpawnHeight = 2000;
    }

    // Update is called once per frame
    void Update()
    {
        playerSpeed = GameControllerScript.playerSpeed;

        if (spawnrate <= 0)
        {
            SpawnObjects();
            spawnrate = 2;
        }
        else
            spawnrate -= Time.deltaTime * (playerSpeed / 100);
    }

    void SpawnObjects()
    {
        //trees alongside the road
        //left
        tempSpawnPosition.Set(Random.Range(spawnzoneTrees[0], spawnzoneTrees[1]), Random.Range(objectSpawnHeight, objectSpawnHeight + 1000), Random.Range(1300, 2200));
        Instantiate(spawnableObjects[0], tempSpawnPosition, Quaternion.identity);

        //right
        tempSpawnPosition.Set(Random.Range(spawnzoneTrees[0] * -1, spawnzoneTrees[1] * -1), Random.Range(objectSpawnHeight, objectSpawnHeight + 1000), Random.Range(1300, 2200));
        Instantiate(spawnableObjects[0], tempSpawnPosition, Quaternion.identity);

        //clouds
        for (int i = 0; i < 7; i++)
        {
            tempSpawnPosition.Set(Random.Range(-3500, 3500), Random.Range(20, 40), 2500);
            Instantiate(spawnableObjects[1], tempSpawnPosition, Quaternion.identity);
        }

        //cars
        //1 or 2
        int tempAmountCars = Random.Range(1, 3);
        int laneIsUsed = -1;
        for (int i = 0; i < tempAmountCars; i++)
        {
            int laneSpawn = Random.Range(1, 4);
            while (laneSpawn == laneIsUsed)
                laneSpawn = Random.Range(1, 4);

            laneIsUsed = laneSpawn;

            switch (laneSpawn)
            {
                case 1:
                    tempSpawnPosition.Set(35, objectSpawnHeight, 3000);

                    break;
                case 2:
                    tempSpawnPosition.Set(0,objectSpawnHeight, 3000);
                    break;
                case 3:
                    tempSpawnPosition.Set(-35, objectSpawnHeight, 3000);
                    break;
                default:
                    break;
            }

            int randomCarModel = Random.Range(2, 4);
            Instantiate(spawnableObjects[randomCarModel], tempSpawnPosition, Quaternion.Euler(0, -90, 0));
        }
    }
}
