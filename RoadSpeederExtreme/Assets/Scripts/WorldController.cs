using UnityEngine;
using System.Collections;

public class WorldController : MonoBehaviour
{

    [SerializeField]
    GameObject[] spawnableObjects;

    [SerializeField]
    GameObject[] spawnableTraffic;

    GameObject scene;
    GameObject Traffic;

    GameController GameControllerScript;

    enum landscapePresets { forest, city, desert };
    [SerializeField]
    landscapePresets currentLandscape;

    [SerializeField]
    int[] spawnzoneTrees;
    int objectSpawnHeight;

    float playerSpeed;
    float spawnrate = 0; //seconds
    float landscapePresetDuration; //seconds

    Vector3 tempSpawnPosition;

    // Use this for initialization
    void Start()
    {
        scene = GameObject.Find("scene");
        Traffic = GameObject.Find("Traffic");

        GameControllerScript = GameObject.Find("GameScripts").GetComponent<GameController>();
        playerSpeed = GameControllerScript.playerSpeed;
        
        objectSpawnHeight = 2000;

        landscapePresetDuration = Random.Range(20, 21);
        currentLandscape = (landscapePresets)Random.Range(0, 3);
    }

    // Update is called once per frame
    void Update()
    {
        playerSpeed = GameControllerScript.playerSpeed;

        //landscape control
        if (landscapePresetDuration <= 0)
        {
            int randomPreset = Random.Range(0, 3);
            switch (randomPreset)
            {
                case 0:
                    if (currentLandscape == landscapePresets.city)
                        goto case 1;
                    currentLandscape = landscapePresets.city;
                    break;
                case 1:
                    if (currentLandscape == landscapePresets.forest)
                        goto case 2;
                    currentLandscape = landscapePresets.forest;
                    break;
                case 2:
                    if (currentLandscape == landscapePresets.desert)
                        goto case 0;
                    currentLandscape = landscapePresets.desert;
                    break;
                default:
                    break;

            }
            landscapePresetDuration = Random.Range(15 - playerSpeed / 100, 21 - playerSpeed / 100);
        }
        else
            landscapePresetDuration -= Time.deltaTime;

        //Spawning
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
        GameObject lastCreatedObject;

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
                    tempSpawnPosition.Set(35, 7, 3000);

                    break;
                case 2:
                    tempSpawnPosition.Set(0, 7, 3000);
                    break;
                case 3:
                    tempSpawnPosition.Set(-35, 7, 3000);
                    break;
                default:
                    break;
            }

            int randomCarModel = Random.Range(0, spawnableTraffic.Length);
            lastCreatedObject = Instantiate(spawnableTraffic[randomCarModel], tempSpawnPosition, Quaternion.Euler(0, -90, 0)) as GameObject;
            lastCreatedObject.transform.SetParent(Traffic.transform);
        }

        switch (currentLandscape)
        {
            case landscapePresets.forest:
                //trees alongside the road
                //trees and bushes left
                for (int i = 0; i < 20; i++)
                {
                    //tree
                    tempSpawnPosition.Set(Random.Range(80, 3000), Random.Range(30, 40), Random.Range(300, 1200));
                    lastCreatedObject = Instantiate(spawnableObjects[0], tempSpawnPosition, Quaternion.identity) as GameObject;
                    lastCreatedObject.transform.SetParent(scene.transform);

                    //bush
                    tempSpawnPosition.Set(Random.Range(80, 3000), 7.8f, Random.Range(1300, 2200));
                    lastCreatedObject = Instantiate(spawnableObjects[2], tempSpawnPosition, Quaternion.identity) as GameObject;
                    lastCreatedObject.transform.SetParent(scene.transform);
                }
                //trees and bushes right
                for (int i = 0; i < 20; i++)
                {
                    //tree
                    tempSpawnPosition.Set(Random.Range(-80, -3000), Random.Range(30, 40), Random.Range(300, 1200));
                    lastCreatedObject = Instantiate(spawnableObjects[0], tempSpawnPosition, Quaternion.identity) as GameObject;
                    lastCreatedObject.transform.SetParent(scene.transform);

                    //bush
                    tempSpawnPosition.Set(Random.Range(-80, -3000), 7.8f, Random.Range(300, 1200));
                    lastCreatedObject = Instantiate(spawnableObjects[2], tempSpawnPosition, Quaternion.identity) as GameObject;
                    lastCreatedObject.transform.SetParent(scene.transform);
                }

                //

                //clouds
                for (int i = 0; i < 7; i++)
                {
                    tempSpawnPosition.Set(Random.Range(-2500, 2500), Random.Range(160, 220), 1500);
                    lastCreatedObject = Instantiate(spawnableObjects[1], tempSpawnPosition, Quaternion.identity) as GameObject;
                    lastCreatedObject.transform.SetParent(scene.transform);
                }
                break;
            case landscapePresets.city:
                //clouds
                for (int i = 0; i < 7; i++)
                {
                    tempSpawnPosition.Set(Random.Range(-2500, 2500), Random.Range(160, 220), 1500);
                    lastCreatedObject = Instantiate(spawnableObjects[1], tempSpawnPosition, Quaternion.identity) as GameObject;
                    lastCreatedObject.transform.SetParent(scene.transform);
                }
                break;
            case landscapePresets.desert:
                //clouds
                for (int i = 0; i < 7; i++)
                {
                    tempSpawnPosition.Set(Random.Range(-2500, 2500), Random.Range(160, 220), 1500);
                    lastCreatedObject = Instantiate(spawnableObjects[1], tempSpawnPosition, Quaternion.identity) as GameObject;
                    lastCreatedObject.transform.SetParent(scene.transform);
                }
                break;
            default:
                break;
        }
        
    }
}
