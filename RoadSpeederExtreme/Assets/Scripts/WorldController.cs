using UnityEngine;
using System.Collections;

public class WorldController : MonoBehaviour
{

    [SerializeField]
    GameObject[] spawnableNature;

    [SerializeField]
    GameObject[] spawnableTraffic;

    [SerializeField]
    GameObject[] spawnableBuildings;

    GameObject scene;
    GameObject Traffic;
    GameObject lastCreatedObject;

    GameController GameControllerScript;

    enum landscapePresets { forest, city, desert, ocean };
    [SerializeField]
    landscapePresets currentLandscape;

    [SerializeField]
    public GameObject[] LandscapeTerrain;

    [SerializeField]
    int[] spawnzoneTrees;

    float playerSpeed;
    float spawnrate = 0; //seconds
    float landscapePresetDuration; //seconds

    Vector3 tempSpawnPosition;

    int[] graden = { 0, 90, 180, 270 };
    // Use this for initialization
    void Start()
    {
        scene = GameObject.Find("scene");
        Traffic = GameObject.Find("Traffic");

        GameControllerScript = GameObject.Find("GameScripts").GetComponent<GameController>();
        playerSpeed = GameControllerScript.playerSpeed;

        landscapePresetDuration = Random.Range(20, 30);
        currentLandscape = (landscapePresets)Random.Range(0, 3);
        ChangeLandscapeTerrain();

        //clouds
        for (int i = 0; i < 250; i++)
        {
            tempSpawnPosition.Set(Random.Range(-4000, 4000), Random.Range(680, 700), Random.Range(-3000, 2800));
            lastCreatedObject = Instantiate(spawnableNature[1], tempSpawnPosition, Quaternion.identity) as GameObject;
            lastCreatedObject.transform.SetParent(scene.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        playerSpeed = GameControllerScript.playerSpeed;

        //landscape control
        if (landscapePresetDuration <= 0)
        {
            int randomPreset = Random.Range(0, 4);
            switch (randomPreset)
            {
                case 0:
                    if (currentLandscape == landscapePresets.city)
                        goto case 1;
                    currentLandscape = landscapePresets.city;
                    landscapePresetDuration = Random.Range(20 - playerSpeed / 100, 30 - playerSpeed / 100);
                    break;
                case 1:
                    if (currentLandscape == landscapePresets.forest)
                        goto case 2;
                    currentLandscape = landscapePresets.forest;
                    landscapePresetDuration = Random.Range(20 - playerSpeed / 100, 30 - playerSpeed / 100);
                    break;
                case 2:
                    if (currentLandscape == landscapePresets.desert)
                        goto case 3;
                    currentLandscape = landscapePresets.desert;
                    landscapePresetDuration = Random.Range(20 - playerSpeed / 100, 30 - playerSpeed / 100);
                    break;
                case 3:
                    if (currentLandscape == landscapePresets.ocean)
                        goto case 0;
                    currentLandscape = landscapePresets.ocean;
                    landscapePresetDuration = 3;
                    break;
                default:
                    break;
            }
            ChangeLandscapeTerrain();
            spawnrate = 2;
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

    void ChangeLandscapeTerrain()
    {
        var currentTerrainNumber = -1;
        switch (currentLandscape)
        {
            case landscapePresets.city:
                var pos = LandscapeTerrain[0].transform.position;
                pos.y = 0;
                LandscapeTerrain[0].transform.position = pos;

                LandscapeTerrain[0].SetActive(true);

                currentTerrainNumber = 0;
                break;
            case landscapePresets.forest:
                pos = LandscapeTerrain[1].transform.position;
                pos.y = 0;
                LandscapeTerrain[1].transform.position = pos;

                LandscapeTerrain[1].SetActive(true);

                currentTerrainNumber = 1;
                break;
            case landscapePresets.desert:
                pos = LandscapeTerrain[2].transform.position;
                pos.y = 0;
                LandscapeTerrain[2].transform.position = pos;

                LandscapeTerrain[2].SetActive(true);

                currentTerrainNumber = 2;
                break;
            case landscapePresets.ocean:
                pos = LandscapeTerrain[3].transform.position;
                pos.y = 0;
                LandscapeTerrain[3].transform.position = pos;

                LandscapeTerrain[3].SetActive(true);

                currentTerrainNumber = 3;
                break;
        }
        for (int i = 0; i < 4; i++)
        {
            if (i != currentTerrainNumber)
            {
                var pos = LandscapeTerrain[i].transform.position;
                pos.y = -1;
                LandscapeTerrain[i].transform.position = pos;
            }
        }
    }

    void SpawnObjects()
    {
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
                    tempSpawnPosition.Set(35, 7, 2000);

                    break;
                case 2:
                    tempSpawnPosition.Set(0, 7, 2000);
                    break;
                case 3:
                    tempSpawnPosition.Set(-35, 7, 2000);
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
                for (int i = 0; i < 50; i++)
                {
                    //tree
                    tempSpawnPosition.Set(Random.Range(80, 3000), Random.Range(30, 40), Random.Range(-900, 0));
                    lastCreatedObject = Instantiate(spawnableNature[0], tempSpawnPosition, Quaternion.Euler(0, Random.Range(0, 360), 0)) as GameObject;
                    lastCreatedObject.transform.SetParent(scene.transform);

                    //bush
                    tempSpawnPosition.Set(Random.Range(80, 3000), 7.8f, Random.Range(-900, 0));
                    lastCreatedObject = Instantiate(spawnableNature[2], tempSpawnPosition, Quaternion.Euler(0, Random.Range(0, 360), 0)) as GameObject;
                    lastCreatedObject.transform.SetParent(scene.transform);
                }
                //trees and bushes right
                for (int i = 0; i < 50; i++)
                {
                    //tree
                    tempSpawnPosition.Set(Random.Range(-80, -3000), Random.Range(30, 40), Random.Range(-900, 0));
                    lastCreatedObject = Instantiate(spawnableNature[0], tempSpawnPosition, Quaternion.Euler(0, Random.Range(0, 360), 0)) as GameObject;
                    lastCreatedObject.transform.SetParent(scene.transform);

                    //bush
                    tempSpawnPosition.Set(Random.Range(-80, -3000), 7.8f, Random.Range(-900, 0));
                    lastCreatedObject = Instantiate(spawnableNature[2], tempSpawnPosition, Quaternion.Euler(0, Random.Range(0, 360), 0)) as GameObject;
                    lastCreatedObject.transform.SetParent(scene.transform);
                }
                
                break;
            case landscapePresets.city:
                //buildings
                //buildings left
                //int[] graden = { 0, 90, 180, 270 };
                for (int i = 0; i < 5; i++)
                {
                    //Building
                    tempSpawnPosition.Set(Random.Range(300, 3000), Random.Range(30, 40), Random.Range(-500, 0));
                    lastCreatedObject = Instantiate(spawnableBuildings[0], tempSpawnPosition, Quaternion.Euler(0, graden[Random.Range(0, 3)], 0)) as GameObject;
                    lastCreatedObject.transform.SetParent(scene.transform);
                }
                //buildings right
                for (int i = 0; i < 5; i++)
                {
                    //Building
                    tempSpawnPosition.Set(Random.Range(-300, -3000), Random.Range(30, 40), Random.Range(-500, 0));
                    lastCreatedObject = Instantiate(spawnableBuildings[0], tempSpawnPosition, Quaternion.Euler(0, graden[Random.Range(0, 3)], 0)) as GameObject;
                    lastCreatedObject.transform.SetParent(scene.transform);
                }
                break;
            case landscapePresets.desert:
                //Cactus
                for (int i = 0; i < 10; i++)
                {
                    tempSpawnPosition.Set(Random.Range(80, 3000), 20, Random.Range(-1000, 0));
                    lastCreatedObject = Instantiate(spawnableNature[4], tempSpawnPosition, Quaternion.Euler(0, Random.Range(0, 360), 0)) as GameObject;
                    lastCreatedObject.transform.SetParent(scene.transform);
                    
                    tempSpawnPosition.Set(Random.Range(-80, -3000), 20, Random.Range(-1000, 0));
                    lastCreatedObject = Instantiate(spawnableNature[4], tempSpawnPosition, Quaternion.Euler(0, Random.Range(0, 360), 0)) as GameObject;
                    lastCreatedObject.transform.SetParent(scene.transform);
                }
                //DesertFormation
                for (int i = 0; i < 1; i++)
                {
                    tempSpawnPosition.Set(Random.Range(1000, 3000), 7.8f, Random.Range(-200, 0));
                    lastCreatedObject = Instantiate(spawnableNature[5], tempSpawnPosition, Quaternion.Euler(0, graden[Random.Range(0, 3)], 0)) as GameObject;
                    lastCreatedObject.transform.SetParent(scene.transform);
                    
                    tempSpawnPosition.Set(Random.Range(-1000, -3000), 7.8f, Random.Range(-200, 0));
                    lastCreatedObject = Instantiate(spawnableNature[5], tempSpawnPosition, Quaternion.Euler(0, graden[Random.Range(0, 3)], 0)) as GameObject;
                    lastCreatedObject.transform.SetParent(scene.transform);
                }
                break;
            case landscapePresets.ocean:
                //Bridge
                tempSpawnPosition.Set(0, 0, 0);
                lastCreatedObject = Instantiate(spawnableBuildings[1], tempSpawnPosition, Quaternion.Euler(0, 0, 0)) as GameObject;
                lastCreatedObject.transform.SetParent(scene.transform);

                tempSpawnPosition.Set(0, 0, 405);
                lastCreatedObject = Instantiate(spawnableBuildings[1], tempSpawnPosition, Quaternion.Euler(0, 0, 0)) as GameObject;
                lastCreatedObject.transform.SetParent(scene.transform);
                
                //Waves
                for (int i = 0; i < 1; i++)
                {
                    tempSpawnPosition.Set(Random.Range(250, 1500), 7.8f, Random.Range(-200, 0));
                    lastCreatedObject = Instantiate(spawnableNature[6], tempSpawnPosition, Quaternion.Euler(0, 90, 0)) as GameObject;
                    lastCreatedObject.transform.SetParent(scene.transform);

                    tempSpawnPosition.Set(Random.Range(-250, -1500), 7.8f, Random.Range(-200, 0));
                    lastCreatedObject = Instantiate(spawnableNature[6], tempSpawnPosition, Quaternion.Euler(0, 90, 0)) as GameObject;
                    lastCreatedObject.transform.SetParent(scene.transform);
                }
                break;
            default:
                break;
        }
        
    }
}
