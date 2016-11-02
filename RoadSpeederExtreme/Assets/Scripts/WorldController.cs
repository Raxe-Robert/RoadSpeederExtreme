using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldController : MonoBehaviour
{
    [SerializeField]
    GameObject[] spawnableNature;

    [SerializeField]
    GameObject[] spawnableTraffic;

    [SerializeField]
    GameObject[] spawnableBuildings;

    GameObject scene;
    GameObject lastCreatedObject;

    GameObject pool_Traffic;
    GameObject pool_Clouds;
    GameObject pool_Trees;
    GameObject pool_Bushes;
    GameObject pool_Buildings;
    GameObject pool_Cactuses;
    GameObject pool_DesertFormations;
    GameObject pool_Bridges;
    GameObject pool_Waves;
    
    public List<List<GameObject>> poolListsContainer;
    List<GameObject> TrafficList;
    List<GameObject> CloudsList;
    List<GameObject> TreesList;
    List<GameObject> BushesList;
    List<GameObject> BuildingsList;
    List<GameObject> CactusesList;
    List<GameObject> DesertFormationsList;
    List<GameObject> BridgesList;
    List<GameObject> WavesList;
    

    GameController GameControllerScript;

    enum landscapePresets { forest, city, desert, ocean };
    
    landscapePresets previousLandscape;
    [SerializeField]
    landscapePresets currentLandscape;

    [SerializeField]
    public GameObject[] LandscapeTerrain;

    float playerSpeed;
    float spawnrateTerrain = 2; //seconds
    float spawnrateRoad = 2;
    float landscapeDuration; //seconds

    Vector3 tempSpawnPosition;

    //int[] degrees = { 0, 90, 180, 270 };
    // Use this for initialization
    void Start()
    {
        scene = GameObject.Find("scene");

        GameControllerScript = GameObject.Find("GameScripts").GetComponent<GameController>();

        pool_Traffic = GameObject.Find("pool_Traffic");
        pool_Clouds = GameObject.Find("pool_Clouds");
        pool_Trees = GameObject.Find("pool_Trees"); ;
        pool_Bushes = GameObject.Find("pool_Bushes"); ;
        pool_Buildings = GameObject.Find("pool_Buildings");
        pool_Cactuses = GameObject.Find("pool_Cactuses");
        pool_DesertFormations = GameObject.Find("pool_DesertFormations");
        pool_Bridges = GameObject.Find("pool_Bridges");
        pool_Waves = GameObject.Find("pool_Waves");

        TrafficList = new List<GameObject>();
        CloudsList = new List<GameObject>();
        TreesList = new List<GameObject>();
        BushesList = new List<GameObject>();
        BuildingsList = new List<GameObject>();
        CactusesList = new List<GameObject>();
        DesertFormationsList = new List<GameObject>();
        BridgesList = new List<GameObject>();
        WavesList = new List<GameObject>();

        poolListsContainer = new List<List<GameObject>>();
        poolListsContainer.Add(TrafficList);
        poolListsContainer.Add(CloudsList);
        poolListsContainer.Add(TreesList);
        poolListsContainer.Add(BushesList);
        poolListsContainer.Add(BuildingsList);
        poolListsContainer.Add(CactusesList);
        poolListsContainer.Add(DesertFormationsList);
        poolListsContainer.Add(BridgesList);
        poolListsContainer.Add(WavesList);

        //Populate pools
        PopulatePool(spawnableTraffic, pool_Traffic, TrafficList, 20, false);
        PopulatePool(spawnableNature[1], pool_Clouds, CloudsList, 150, true);
        PopulatePool(spawnableNature[0], pool_Trees, TreesList, 600, false);
        PopulatePool(spawnableNature[2], pool_Bushes, BushesList, 600, false);
        PopulatePool(spawnableBuildings[0], pool_Buildings, BuildingsList, 50, false);
        PopulatePool(spawnableNature[4], pool_Cactuses, CactusesList, 150, false);
        PopulatePool(spawnableNature[5], pool_DesertFormations, DesertFormationsList, 20, false);
        PopulatePool(spawnableBuildings[1], pool_Bridges, BridgesList, 150, false);
        PopulatePool(spawnableNature[6], pool_Waves, WavesList, 20, false);

        playerSpeed = GameControllerScript.playerSpeed;
        landscapeDuration = Random.Range(20, 30);

        ChangeLandscape();

        StartCoroutine(RoadPopulator());
        StartCoroutine(TerrainPopulator());
    }

    // Update is called once per frame
    void Update()
    {
        playerSpeed = GameControllerScript.playerSpeed;

        //landscape control
        if (landscapeDuration <= 0)
        {
            ChangeLandscape();
            spawnrateTerrain = 2;
        }
        else
            landscapeDuration -= Time.deltaTime;
        
    }
    
    IEnumerator RoadPopulator()
    {
        while (true)
        {
            #region Spawning stuff
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
                SpawnObject(TrafficList, tempSpawnPosition);
            }
            #endregion

            float waitTime = spawnrateRoad / (playerSpeed / 100);
            if (waitTime <= 0)
                waitTime = 0.01f;

            yield return new WaitForSeconds(waitTime);
        }
    }

    IEnumerator TerrainPopulator()
    {
        while (true)
        {
            #region Spawning stuff
            switch (currentLandscape)
            {
                case landscapePresets.forest:
                    //trees alongside the road
                    //trees and bushes left
                    for (int i = 0; i < 30; i++)
                    {
                        tempSpawnPosition.Set(Random.Range(80, 2500), 44, Random.Range(-900, 0));
                        SpawnObject(TreesList, tempSpawnPosition);

                        tempSpawnPosition.Set(Random.Range(-80, -2500), 44, Random.Range(-900, 0));
                        SpawnObject(TreesList, tempSpawnPosition);
                    }
                    //trees and bushes right
                    for (int i = 0; i < 30; i++)
                    {
                        tempSpawnPosition.Set(Random.Range(80, 2500), 7.8f, Random.Range(-900, 0));
                        SpawnObject(BushesList, tempSpawnPosition);

                        tempSpawnPosition.Set(Random.Range(-80, -2500), 7.8f, Random.Range(-900, 0));
                        SpawnObject(BushesList, tempSpawnPosition);
                    }

                    break;
                case landscapePresets.city:
                    //buildings
                    //buildings left
                    //int[] graden = { 0, 90, 180, 270 };
                    tempSpawnPosition.Set(Random.Range(350, 2500), Random.Range(30, 40), Random.Range(-500, 0));
                    SpawnObject(BuildingsList, tempSpawnPosition);

                    tempSpawnPosition.Set(Random.Range(80, 2500), 7.8f, Random.Range(-900, 0));
                    SpawnObject(BushesList, tempSpawnPosition);

                    //buildings right
                    tempSpawnPosition.Set(Random.Range(-350, -2500), Random.Range(30, 40), Random.Range(-500, 0));
                    SpawnObject(BuildingsList, tempSpawnPosition);

                    tempSpawnPosition.Set(Random.Range(-80, -2500), 7.8f, Random.Range(-900, 0));
                    SpawnObject(BushesList, tempSpawnPosition);

                    break;
                case landscapePresets.desert:
                    //Cactuses
                    for (int i = 0; i < 3; i++)
                    {
                        tempSpawnPosition.Set(Random.Range(80, 2500), 20, Random.Range(-1000, 0));
                        SpawnObject(CactusesList, tempSpawnPosition);

                        tempSpawnPosition.Set(Random.Range(-80, -2500), 20, Random.Range(-1000, 0));
                        SpawnObject(CactusesList, tempSpawnPosition);
                    }

                    //DesertFormations left or right
                    tempSpawnPosition.Set(Random.Range(1000, 2500), 0, Random.Range(-200, 0));
                    SpawnObject(DesertFormationsList, tempSpawnPosition);

                    tempSpawnPosition.Set(Random.Range(-1000, -2500), 0, Random.Range(-200, 0));
                    SpawnObject(DesertFormationsList, tempSpawnPosition);

                    break;
                case landscapePresets.ocean:
                    //Bridge
                    tempSpawnPosition.Set(0, 0, 0);
                    SpawnObject(BridgesList, tempSpawnPosition);

                    tempSpawnPosition.Set(0, 0, 405);
                    SpawnObject(BridgesList, tempSpawnPosition);

                    //Waves
                    for (int i = 0; i < 1; i++)
                    {
                        tempSpawnPosition.Set(Random.Range(250, 1500), 7.8f, Random.Range(-200, 0));
                        SpawnObject(WavesList, tempSpawnPosition);

                        tempSpawnPosition.Set(Random.Range(-250, -1500), 7.8f, Random.Range(-200, 0));
                        SpawnObject(WavesList, tempSpawnPosition);
                    }
                    break;
                default:
                    break;
            }
            #endregion

            float waitTime = spawnrateTerrain / (playerSpeed / 100);
            if (waitTime <= 0)
                waitTime = 0.01f;

            yield return new WaitForSeconds(waitTime);
        }
    }

    //Set objective active at spawnPosition
    void SpawnObject(List<GameObject> objectList, Vector3 spawnPosition)
    {
        if (objectList.Count > 0)
        {
            var newObject = objectList[objectList.Count - 1];
            newObject.gameObject.transform.position = spawnPosition;
            newObject.gameObject.SetActive(true);
            objectList.RemoveAt(objectList.Count - 1);
        }
        else
        {
            Debug.Log("too few objects");
        }
    }

    //Populate with given object
    void PopulatePool(GameObject gameObject, GameObject objectPool, List<GameObject> objectPoolList, int objectAmount, bool activeOnCreate)
    {
        for (int i = 0; i < objectAmount; i++)
        {
            lastCreatedObject = Instantiate(gameObject, gameObject.transform.position, Quaternion.identity) as GameObject;
            lastCreatedObject.transform.SetParent(objectPool.transform);

            objectPoolList.Add(lastCreatedObject);

            if (!activeOnCreate)
                lastCreatedObject.SetActive(false);
        }
    }

    //Populate with random object from given object list
    void PopulatePool(GameObject[] gameObjectList, GameObject objectPool, List<GameObject> objectPoolList, int objectAmount, bool activeOnCreate)
    {
        for (int i = 0; i < objectAmount; i++)
        {
            var gameObject = gameObjectList[Random.Range(0, gameObjectList.Length)];
            lastCreatedObject = Instantiate(gameObject, gameObject.transform.position, gameObject.transform.rotation) as GameObject;
            lastCreatedObject.transform.SetParent(objectPool.transform);

            objectPoolList.Add(lastCreatedObject);

            if (!activeOnCreate)
                lastCreatedObject.SetActive(false);
        }
    }
    
    //Change landscape preset randomly
    void ChangeLandscape()
    {
        //Random landscape preset
        int randomPreset = Random.Range(0, 4);
        switch (randomPreset)
        {
            case 0:
                if (currentLandscape == landscapePresets.city || previousLandscape == landscapePresets.city)
                    goto case 1;
                previousLandscape = currentLandscape;
                currentLandscape = landscapePresets.city;
                landscapeDuration = Random.Range(20 - playerSpeed / 100, 30 - playerSpeed / 100);
                break;
            case 1:
                if (currentLandscape == landscapePresets.forest || previousLandscape == landscapePresets.forest)
                    goto case 2;
                previousLandscape = currentLandscape;
                currentLandscape = landscapePresets.forest;
                landscapeDuration = Random.Range(20 - playerSpeed / 100, 30 - playerSpeed / 100);
                break;
            case 2:
                if (currentLandscape == landscapePresets.desert || previousLandscape == landscapePresets.desert)
                    goto case 3;
                previousLandscape = currentLandscape;
                currentLandscape = landscapePresets.desert;
                landscapeDuration = Random.Range(20 - playerSpeed / 100, 30 - playerSpeed / 100);
                break;
            case 3:
                if (currentLandscape == landscapePresets.ocean || previousLandscape == landscapePresets.ocean)
                    goto case 0;
                previousLandscape = currentLandscape;
                currentLandscape = landscapePresets.ocean;
                landscapeDuration = 2;
                break;
            default:
                break;
        }

        //Match terrain with landscape
        var currentTerrainNumber = -1;
        switch (currentLandscape)
        {
            case landscapePresets.city:
                var pos = LandscapeTerrain[0].transform.position;
                pos.y = 0f;
                LandscapeTerrain[0].transform.position = pos;

                LandscapeTerrain[0].SetActive(true);

                currentTerrainNumber = 0;
                break;
            case landscapePresets.forest:
                pos = LandscapeTerrain[1].transform.position;
                pos.y = 0f;
                LandscapeTerrain[1].transform.position = pos;

                LandscapeTerrain[1].SetActive(true);

                currentTerrainNumber = 1;
                break;
            case landscapePresets.desert:
                pos = LandscapeTerrain[2].transform.position;
                pos.y = 0f;
                LandscapeTerrain[2].transform.position = pos;

                LandscapeTerrain[2].SetActive(true);

                currentTerrainNumber = 2;
                break;
            case landscapePresets.ocean:
                pos = LandscapeTerrain[3].transform.position;
                pos.y = 0f;
                LandscapeTerrain[3].transform.position = pos;

                LandscapeTerrain[3].SetActive(true);

                currentTerrainNumber = 3;
                break;
        }

        //For all other terrains do:
        for (int i = 0; i < 4; i++)
        {
            if (i != currentTerrainNumber && LandscapeTerrain[i].transform.position.z <= 1000)
            {
                var pos = LandscapeTerrain[i].transform.position;
                pos.y = -0.25f;

                if (LandscapeTerrain[i].activeInHierarchy == false)
                {
                    pos.z = -5000;
                }

                LandscapeTerrain[i].transform.position = pos;
            }
        }
    }
}
