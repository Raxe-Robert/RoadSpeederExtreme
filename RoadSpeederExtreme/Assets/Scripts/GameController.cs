using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    public int playerSpeed;

    float speedIncreaseTimer;

	// Use this for initialization
	void Start () {
        playerSpeed = 130;
        speedIncreaseTimer = 0;

    }
	
	// Update is called once per frame
	void Update () {
        if (speedIncreaseTimer >= 0.4)
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
