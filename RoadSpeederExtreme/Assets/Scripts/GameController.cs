using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    public int playerSpeed;
    public float playerScore;

    float speedIncreaseTimer;

	// Use this for initialization
	void Start () {
        playerSpeed = 130;
        playerScore = 0;
        speedIncreaseTimer = 0;

    }
	
	// Update is called once per frame
	void Update () {
        //Increase speed every     v   by 1
        if (speedIncreaseTimer >= 0.2)
        {
            playerSpeed++;
            playerScore++;
            speedIncreaseTimer = 0;
        }
        else
        {
            speedIncreaseTimer += Time.deltaTime;
        }
	}
}
