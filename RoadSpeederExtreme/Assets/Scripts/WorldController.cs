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
    GameObject pool_Rollers;

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
    List<GameObject> RollerList;
    

    GameController GameControllerScript;

    enum landscapePresets { forest, city, desert, ocean };
    
    landscapePresets previousLandscape;
    [SerializeField]
    landscapePresets currentLandscape;

    [SerializeField]
    public GameObject[] LandscapeTerrain;

    float playerSpeed;
    float spawnrateTerrain = 1f; //seconds
    float spawnrateRoad = 2f;
    float spawnrateRoller = 30f;
    float landscapeDuration; //seconds

    Vector3 tempSpawnPosition;
    Quaternion tempSpawnRotation;

    // Use this for initialization
    void Start()
    {
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
        pool_Rollers = GameObject.Find("pool_Rollers");

        TrafficList = new List<GameObject>();
        CloudsList = new List<GameObject>();
        TreesList = new List<GameObject>();
        BushesList = new List<GameObject>();
        BuildingsList = new List<GameObject>();
        CactusesList = new List<GameObject>();
        DesertFormationsList = new List<GameObject>();
        BridgesList = new List<GameObject>();
        WavesList = new List<GameObject>();
        RollerList = new List<GameObject>();

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
        poolListsContainer.Add(RollerList);

        //Populate pools
        PopulatePool(spawnableTraffic, pool_Traffic, TrafficList, 20, false);
        PopulatePool(spawnableNature[1], pool_Clouds, CloudsList, 150, true);
        PopulatePool(spawnableNature[0], pool_Trees, TreesList, 360, false);
        PopulatePool(spawnableNature[2], pool_Bushes, BushesList, 360, false);
        PopulatePool(spawnableBuildings[0], pool_Buildings, BuildingsList, 15, false);
        PopulatePool(spawnableNature[4], pool_Cactuses, CactusesList, 70, false);
        PopulatePool(spawnableNature[5], pool_DesertFormations, DesertFormationsList, 12, false);
        PopulatePool(spawnableBuildings[1], pool_Bridges, BridgesList, 15, false);
        PopulatePool(spawnableNature[6], pool_Waves, WavesList, 15, false);
        PopulatePool(spawnableNature[3], pool_Rollers, RollerList, 5, false);

        playerSpeed = GameControllerScript.playerSpeed;
        landscapeDuration = Random.Range(20, 30);

        ChangeLandscape();

        StartCoroutine(RoadPopulator());
        StartCoroutine(TerrainPopulator());
        StartCoroutine(BonusItemSpawner());
    }

    // Update is called once per frame
    void Update()
    {
        playerSpeed = GameControllerScript.playerSpeed;

        //landscape control
        if (landscapeDuration <= 0)
        {
            ChangeLandscape();
            spawnrateTerrain = 1f;
        }
        else
            landscapeDuration -= Time.deltaTime;
        
    }

    IEnumerator BonusItemSpawner()
    {
        while (true)
        {
            if (currentLandscape == landscapePresets.desert)
            {
                tempSpawnPosition.Set(Random.Range(-5f, -10f), 0.7f, Random.Range(300f, 450f));
                SpawnObject(RollerList, tempSpawnPosition, tempSpawnRotation);
            }
            float waitTime = spawnrateRoller / (playerSpeed / 70);
            if (waitTime <= 0)
                waitTime = 0.1f;

            yield return new WaitForSeconds(waitTime);
        }
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
            tempSpawnRotation = new Quaternion(0, -90, 0, 90);
            for (int i = 0; i < tempAmountCars; i++)
            {
                int laneSpawn = Random.Range(1, 4);
                while (laneSpawn == laneIsUsed)
                    laneSpawn = Random.Range(1, 4);

                laneIsUsed = laneSpawn;

                switch (laneSpawn)
                {
                    case 1:
                        tempSpawnPosition.Set(3.5f, 0.7f, 200);
                        break;
                    case 2:
                        tempSpawnPosition.Set(0, 0.7f, 200);
                        break;
                    case 3:
                        tempSpawnPosition.Set(-3.5f, 0.7f, 200);
                        break;
                    default:
                        break;
                }
                SpawnObject(TrafficList, tempSpawnPosition, tempSpawnRotation);
            }
            #endregion

            float waitTime = spawnrateRoad / (playerSpeed / 70);
            if (waitTime <= 0)
                waitTime = 0.1f;

            yield return new WaitForSeconds(waitTime);
        }
    }

    IEnumerator TerrainPopulator()
    {
        while (true)
        {
            int[] buildinglength = { 4, 19, 23, 40 };
            int[] degrees = { 0, 90, 180, 270 };
            #region Spawning stuff
            switch (currentLandscape)
            {
                case landscapePresets.forest:
                    //Trees
                    for (int i = 0; i < 30; i++)
                    {
                        tempSpawnPosition.Set(Random.Range(8, 250), 4, Random.Range(-90, 0));
                        tempSpawnRotation.Set(0, Random.Range(0, 360), 0, tempSpawnRotation.w);
                        SpawnObject(TreesList, tempSpawnPosition, tempSpawnRotation);

                        tempSpawnPosition.Set(Random.Range(-8, -250), 4, Random.Range(-90, 0));
                        tempSpawnRotation.Set(0, Random.Range(0, 360), 0, tempSpawnRotation.w);
                        SpawnObject(TreesList, tempSpawnPosition, tempSpawnRotation);
                    }
                    //Bushes
                    for (int i = 0; i < 30; i++)
                    {
                        tempSpawnPosition.Set(Random.Range(8, 250), 1f, Random.Range(-90, 0));
                        tempSpawnRotation.Set(0, Random.Range(0, 360), 0, tempSpawnRotation.w);
                        SpawnObject(BushesList, tempSpawnPosition, tempSpawnRotation);

                        tempSpawnPosition.Set(Random.Range(-8, -250), 1f, Random.Range(-90, 0));
                        tempSpawnRotation.Set(0, Random.Range(0, 360), 0, tempSpawnRotation.w);
                        SpawnObject(BushesList, tempSpawnPosition, tempSpawnRotation);
                    }

                    break;
                case landscapePresets.city:
                    //buildings left
                    tempSpawnPosition.Set(Random.Range(35, 250), buildinglength[Random.Range(0, 3)], Random.Range(-50, 0));
                    tempSpawnRotation.Set(0, degrees[Random.Range(0, 3)], 0, tempSpawnRotation.w);
                    SpawnObject(BuildingsList, tempSpawnPosition, tempSpawnRotation);

                    tempSpawnPosition.Set(Random.Range(8, 250), 0.2f, Random.Range(-90, 0));
                    tempSpawnRotation.Set(0, Random.Range(0, 360), 0, tempSpawnRotation.w);
                    SpawnObject(BushesList, tempSpawnPosition, tempSpawnRotation);

                    //buildings right
                    tempSpawnPosition.Set(Random.Range(-35, -250), buildinglength[Random.Range(0, 3)], Random.Range(-50, 0));
                    tempSpawnRotation.Set(0, degrees[Random.Range(0, 3)], 0, tempSpawnRotation.w);
                    SpawnObject(BuildingsList, tempSpawnPosition, tempSpawnRotation);

                    tempSpawnPosition.Set(Random.Range(-8, -250), 0.2f, Random.Range(-90, 0));
                    tempSpawnRotation.Set(0, Random.Range(0, 360), 0, tempSpawnRotation.w);
                    SpawnObject(BushesList, tempSpawnPosition, tempSpawnRotation);

					//Trees
					for (int i = 0; i < 5; i++)
					{
						tempSpawnPosition.Set(Random.Range(8, 250), 4, Random.Range(-90, 0));
						tempSpawnRotation.Set(0, Random.Range(0, 360), 0, tempSpawnRotation.w);
						SpawnObject(TreesList, tempSpawnPosition, tempSpawnRotation);

						tempSpawnPosition.Set(Random.Range(-8, -250), 4, Random.Range(-90, 0));
						tempSpawnRotation.Set(0, Random.Range(0, 360), 0, tempSpawnRotation.w);
						SpawnObject(TreesList, tempSpawnPosition, tempSpawnRotation);
					}
                    break;
                case landscapePresets.desert:
                    //Cactuses
                    for (int i = 0; i < 5; i++)
                    {
                        tempSpawnRotation.Set(0, Random.Range(0, 360), 0, tempSpawnRotation.w);

                        tempSpawnPosition.Set(Random.Range(8, 250), 2, Random.Range(-100, 0));
                        SpawnObject(CactusesList, tempSpawnPosition, tempSpawnRotation);

                        tempSpawnPosition.Set(Random.Range(-8, -250), 2, Random.Range(-100, 0));
                        SpawnObject(CactusesList, tempSpawnPosition, tempSpawnRotation);
                    }

                    //DesertFormations left or right
                    tempSpawnRotation.Set(0, degrees[Random.Range(0, 3)], 0, tempSpawnRotation.w);
                    tempSpawnPosition.Set(Random.Range(100, 250), 0, Random.Range(-20, 0));
                    SpawnObject(DesertFormationsList, tempSpawnPosition, tempSpawnRotation);

                    tempSpawnPosition.Set(Random.Range(-100, -250), 0, Random.Range(-20, 0));
                    SpawnObject(DesertFormationsList, tempSpawnPosition, tempSpawnRotation);

                    break;
                case landscapePresets.ocean:
                    //Bridge
                    tempSpawnPosition.Set(0, 0, 0);
                    tempSpawnRotation.Set(0, 0, 0, tempSpawnRotation.w);
                    SpawnObject(BridgesList, tempSpawnPosition, tempSpawnRotation);

                    tempSpawnPosition.Set(0, 0, 40.5f);
                    SpawnObject(BridgesList, tempSpawnPosition, tempSpawnRotation);

                    //Waves
                    for (int i = 0; i < 1; i++)
                    {
                        tempSpawnPosition.Set(Random.Range(25, 150), 1f, Random.Range(-20, 0));
                        tempSpawnRotation.Set(0, 90, 0, tempSpawnRotation.w);
                        SpawnObject(WavesList, tempSpawnPosition, tempSpawnRotation);

                        tempSpawnPosition.Set(Random.Range(-25, -150), 1f, Random.Range(-20, 0));
                        SpawnObject(WavesList, tempSpawnPosition, tempSpawnRotation);
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
    void SpawnObject(List<GameObject> objectList, Vector3 spawnPosition, Quaternion spawnRotation)
    {
        if (objectList.Count > 0)
        {
            var newObject = objectList[objectList.Count - 1];
            newObject.gameObject.transform.position = spawnPosition;
            newObject.gameObject.transform.rotation = spawnRotation;
            newObject.gameObject.SetActive(true);
            objectList.RemoveAt(objectList.Count - 1);
        }
        else
        {
            Debug.Log("too few objects:" + objectList.Count);
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
            if (i != currentTerrainNumber && LandscapeTerrain[i].transform.position.z <= 100)
            {
                var pos = LandscapeTerrain[i].transform.position;
                pos.y = -0.25f;

                if (LandscapeTerrain[i].activeInHierarchy == false)
                {
                    pos.z = -500;
                }

                LandscapeTerrain[i].transform.position = pos;
            }
        }
    }
}
