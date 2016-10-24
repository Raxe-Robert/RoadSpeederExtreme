using UnityEngine;
using System.Collections;

public class WorldController : MonoBehaviour {

    [SerializeField]
	GameObject[] spawnableObjects;

    GameController GameControllerScript;

    [SerializeField]
    int[] spawnzoneTrees;

    float playerSpeed;
    float spawnrate = 2; //seconds

    Vector3 tempSpawnPosition;

    // Use this for initialization
    void Start () {
        GameControllerScript = GameObject.Find("GameScripts").GetComponent<GameController>();
        playerSpeed = GameControllerScript.playerSpeed;
    }
	
	// Update is called once per frame
	void Update () {
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
        Debug.Log("spawn");

        //trees alongside the road
        //left
        tempSpawnPosition.Set(Random.Range(spawnzoneTrees[0], spawnzoneTrees[1]), 0, Random.Range(2800, 3200));
        Instantiate(spawnableObjects[0], tempSpawnPosition, Quaternion.identity);

        //right
        tempSpawnPosition.Set(Random.Range(spawnzoneTrees[0] * -1, spawnzoneTrees[1] * -1), 0, Random.Range(2800, 3200));
        Instantiate(spawnableObjects[0], tempSpawnPosition, Quaternion.identity);

        //clouds
        for(int i = 0; i < 4; i++)
        {
            tempSpawnPosition.Set(Random.Range(-3500, 3500), 0, 2500);
            Instantiate(spawnableObjects[1], tempSpawnPosition, Quaternion.identity);

        }
    }
}
