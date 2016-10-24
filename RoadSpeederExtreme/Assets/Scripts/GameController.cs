using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    public int playerSpeed;

    float speedIncreaseTimer;

	// Use this for initialization
	void Start () {
        playerSpeed = 100;
        speedIncreaseTimer = 0;

    }
	
	// Update is called once per frame
	void Update () {
        if (speedIncreaseTimer >= 1)
        {
            playerSpeed++;
            speedIncreaseTimer = 0;
        }
        else
        {
            speedIncreaseTimer += Time.deltaTime;
        }
	}
}
