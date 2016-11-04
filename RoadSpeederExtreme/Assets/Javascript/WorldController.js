import UnityEngine;
import System.Collections;
import System.Collections.Generic;

public class WorldController extends MonoBehaviour
{
    @SerializeField
    var spawnableNature: GameObject[];

    @SerializeField
    var spawnableTraffic: GameObject[];

    @SerializeField
    var spawnableBuildings: GameObject[];

    private var scene: GameObject;
    private var lastCreatedObject: GameObject;

    private var pool_Traffic: GameObject;
    private var pool_Clouds: GameObject;
    private var pool_Trees: GameObject;
    private var pool_Bushes: GameObject;
    private var pool_Buildings: GameObject;
    private var pool_Cactuses: GameObject;
    private var pool_DesertFormations: GameObject;
    private var pool_Bridges: GameObject;
    private var pool_Waves: GameObject;
    
    public var poolListsContainer: List.<List.<GameObject> >;
    private var TrafficList: List.<GameObject>;
    private var CloudsList: List.<GameObject>;
    private var TreesList: List.<GameObject>;
    private var BushesList: List.<GameObject>;
    private var BuildingsList: List.<GameObject>;
    private var CactusesList: List.<GameObject>;
    private var DesertFormationsList: List.<GameObject>;
    private var BridgesList: List.<GameObject>;
    private var WavesList: List.<GameObject>;
    

    private var GameControllerScript: GameController;

    enum landscapePresets { forest, city, desert, ocean };
    
    private var previousLandscape: landscapePresets;
    @SerializeField
    var currentLandscape: landscapePresets;

    @SerializeField
    public var LandscapeTerrain: GameObject[];

    private var playerSpeed: float;
    private var spawnrateTerrain: float = 1; //seconds
    private var spawnrateRoad: float = 2;
    private var landscapeDuration: float; //seconds

    private var tempSpawnPosition: Vector3;
    private var tempSpawnRotation: Quaternion;

    // Use this for initialization
    private function Start()
    {
        scene = GameObject.Find("scene");

        GameControllerScript = GameObject.Find("GameScripts").GetComponent.<GameController>();

        pool_Traffic = GameObject.Find("pool_Traffic");
        pool_Clouds = GameObject.Find("pool_Clouds");
        pool_Trees = GameObject.Find("pool_Trees"); ;
        pool_Bushes = GameObject.Find("pool_Bushes"); ;
        pool_Buildings = GameObject.Find("pool_Buildings");
        pool_Cactuses = GameObject.Find("pool_Cactuses");
        pool_DesertFormations = GameObject.Find("pool_DesertFormations");
        pool_Bridges = GameObject.Find("pool_Bridges");
        pool_Waves = GameObject.Find("pool_Waves");

        TrafficList = new List.<GameObject>();
        CloudsList = new List.<GameObject>();
        TreesList = new List.<GameObject>();
        BushesList = new List.<GameObject>();
        BuildingsList = new List.<GameObject>();
        CactusesList = new List.<GameObject>();
        DesertFormationsList = new List.<GameObject>();
        BridgesList = new List.<GameObject>();
        WavesList = new List.<GameObject>();

        poolListsContainer = new List.<List.<GameObject> >();
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
        PopulatePool(spawnableNature[0], pool_Trees, TreesList, 400, false);
        PopulatePool(spawnableNature[2], pool_Bushes, BushesList, 400, false);
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
    private function Update()
    {
        playerSpeed = GameControllerScript.playerSpeed;

        //landscape control
        if (landscapeDuration <= 0)
        {
            ChangeLandscape();
            spawnrateTerrain = 1;
        }
        else
            landscapeDuration -= Time.deltaTime;
        
    }
    
    private function RoadPopulator(): IEnumerator
    {
        while (true)
        {
            
            //cars
            //1 or 2
            var tempAmountCars: int = Random.Range(1, 3);
            var laneIsUsed: int = -1;
            tempSpawnRotation = new Quaternion(0, -90, 0, 90);
            for (var i: int = 0; i < tempAmountCars; i++)
            {
                var laneSpawn: int = Random.Range(1, 4);
                while (laneSpawn == laneIsUsed)
                    laneSpawn = Random.Range(1, 4);

                laneIsUsed = laneSpawn;

                switch (laneSpawn)
                {
                    case 1:
                        tempSpawnPosition.Set(3.5, 0.7, 200);
                        break;
                    case 2:
                        tempSpawnPosition.Set(0, 0.7, 200);
                        break;
                    case 3:
                        tempSpawnPosition.Set(-3.5, 0.7, 200);
                        break;
                    default:
                        break;
                }
                SpawnObject(TrafficList, tempSpawnPosition, tempSpawnRotation);
            }
            

            var waitTime: float = spawnrateRoad / (playerSpeed / 70);
            if (waitTime <= 0)
                waitTime = 0.1;

            yield  WaitForSeconds(waitTime);
        }
    }

    private function TerrainPopulator(): IEnumerator
    {
        while (true)
        {
            var buildinglength: int[] = [ 4, 19, 23, 40 ];
            var degrees: int[] = [ 0, 90, 180, 270 ];
            
            switch (currentLandscape)
            {
                case landscapePresets.forest:
                    //Bushes
                    for (var i: int = 0; i < 30; i++)
                    {
                        tempSpawnPosition.Set(Random.Range(8, 250), 4, Random.Range(-90, 0));
                        tempSpawnRotation.Set(0, Random.Range(0, 360), 0, tempSpawnRotation.w);
                        SpawnObject(TreesList, tempSpawnPosition, tempSpawnRotation);

                        tempSpawnPosition.Set(Random.Range(-8, -250), 4, Random.Range(-90, 0));
                        tempSpawnRotation.Set(0, Random.Range(0, 360), 0, tempSpawnRotation.w);
                        SpawnObject(TreesList, tempSpawnPosition, tempSpawnRotation);
                    }
                    //trees
                    for (i = 0; i < 30; i++)
                    {
                        tempSpawnPosition.Set(Random.Range(8, 250), 1, Random.Range(-90, 0));
                        tempSpawnRotation.Set(0, Random.Range(0, 360), 0, tempSpawnRotation.w);
                        SpawnObject(BushesList, tempSpawnPosition, tempSpawnRotation);

                        tempSpawnPosition.Set(Random.Range(-8, -250), 1, Random.Range(-90, 0));
                        tempSpawnRotation.Set(0, Random.Range(0, 360), 0, tempSpawnRotation.w);
                        SpawnObject(BushesList, tempSpawnPosition, tempSpawnRotation);
                    }

                    break;
                case landscapePresets.city:
                    //buildings left
                    tempSpawnPosition.Set(Random.Range(35, 250), buildinglength[Random.Range(0, 3)], Random.Range(-50, 0));
                    tempSpawnRotation.Set(0, degrees[Random.Range(0, 3)], 0, tempSpawnRotation.w);
                    SpawnObject(BuildingsList, tempSpawnPosition, tempSpawnRotation);

                    tempSpawnPosition.Set(Random.Range(8, 250), 7.8, Random.Range(-90, 0));
                    tempSpawnRotation.Set(0, degrees[Random.Range(0, 3)], 0, tempSpawnRotation.w);
                    SpawnObject(BushesList, tempSpawnPosition, tempSpawnRotation);

                    //buildings right
                    tempSpawnPosition.Set(Random.Range(-35, -250), buildinglength[Random.Range(0, 3)], Random.Range(-50, 0));
                    tempSpawnRotation.Set(0, degrees[Random.Range(0, 3)], 0, tempSpawnRotation.w);
                    SpawnObject(BuildingsList, tempSpawnPosition, tempSpawnRotation);

                    tempSpawnPosition.Set(Random.Range(-8, -250), 7.8, Random.Range(-90, 0));
                    tempSpawnRotation.Set(0, degrees[Random.Range(0, 3)], 0, tempSpawnRotation.w);
                    SpawnObject(BushesList, tempSpawnPosition, tempSpawnRotation);

                    break;
                case landscapePresets.desert:
                    //Cactuses
                    for (i = 0; i < 3; i++)
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

                    tempSpawnPosition.Set(0, 0, 40.5);
                    SpawnObject(BridgesList, tempSpawnPosition, tempSpawnRotation);

                    //Waves
                    for (i = 0; i < 1; i++)
                    {
                        tempSpawnPosition.Set(Random.Range(25, 150), 1, Random.Range(-20, 0));
                        tempSpawnRotation.Set(0, 90, 0, tempSpawnRotation.w);
                        SpawnObject(WavesList, tempSpawnPosition, tempSpawnRotation);

                        tempSpawnPosition.Set(Random.Range(-25, -150), 1, Random.Range(-20, 0));
                        SpawnObject(WavesList, tempSpawnPosition, tempSpawnRotation);
                    }
                    break;
                default:
                    break;
            }
            

            var waitTime: float = spawnrateTerrain / (playerSpeed / 100);
            if (waitTime <= 0)
                waitTime = 0.01;

            yield  WaitForSeconds(waitTime);
        }
    }

    //Set objective active at spawnPosition
    void SpawnObject(objectList: List.<GameObject>, spawnPosition: Vector3, spawnRotation: Quaternion)
    {
        if (objectList.Count > 0)
        {
            private var newObject = objectList[objectList.Count - 1];
            newObject.gameObject.transform.position = spawnPosition;
            newObject.gameObject.transform.rotation = spawnRotation;
            newObject.gameObject.SetActive(true);
            objectList.RemoveAt(objectList.Count - 1);
        }
        else
        {
            Debug.Log("too few objects");
        }
    }

    //Populate with given object
    void PopulatePool(gameObject: GameObject, objectPool: GameObject, objectPoolList: List.<GameObject>, objectAmount: int, activeOnCreate: boolean)
    {
        for (var i: int = 0; i < objectAmount; i++)
        {
            lastCreatedObject = Instantiate(gameObject, gameObject.transform.position, Quaternion.identity) as GameObject;
            lastCreatedObject.transform.SetParent(objectPool.transform);

            objectPoolList.Add(lastCreatedObject);

            if (!activeOnCreate)
                lastCreatedObject.SetActive(false);
        }
    }

    //Populate with random object from given object list
    void PopulatePool(gameObjectList: GameObject[], objectPool: GameObject, objectPoolList: List.<GameObject>, objectAmount: int, activeOnCreate: boolean)
    {
        for (var i: int = 0; i < objectAmount; i++)
        {
            private var gameObject = gameObjectList[Random.Range(0, gameObjectList.Length)];
            lastCreatedObject = Instantiate(gameObject, gameObject.transform.position, gameObject.transform.rotation) as GameObject;
            lastCreatedObject.transform.SetParent(objectPool.transform);

            objectPoolList.Add(lastCreatedObject);

            if (!activeOnCreate)
                lastCreatedObject.SetActive(false);
        }
    }
    
    //Change landscape preset randomly
    private function ChangeLandscape()
    {
        //Random landscape preset
        var randomPreset: int = Random.Range(0, 4);
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

        //For all other terrains do:
        for (var i: int = 0; i < 4; i++)
        {
            if (i != currentTerrainNumber && LandscapeTerrain[i].transform.position.z <= 100)
            {
                var pos = LandscapeTerrain[i].transform.position;
                pos.y = -0.25;

                if (LandscapeTerrain[i].activeInHierarchy == false)
                {
                    pos.z = -500;
                }

                LandscapeTerrain[i].transform.position = pos;
            }
        }
    }
}
