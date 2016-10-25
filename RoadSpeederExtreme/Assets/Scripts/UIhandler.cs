using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIhandler : MonoBehaviour {

    [SerializeField]
    Text playerSpeed;
    [SerializeField]
    Text playerScore;

    [SerializeField]
    GameController GameControllerScript;

	// Use this for initialization
	void Start () {
        GameControllerScript = GameObject.Find("GameScripts").GetComponent<GameController>();
	}
	
	// Update is called once per frame
	void Update () {
        playerSpeed.text = "Speed: " + GameControllerScript.playerSpeed.ToString();
        playerScore.text = "Score: " + GameControllerScript.playerScore.ToString();
    }
}
