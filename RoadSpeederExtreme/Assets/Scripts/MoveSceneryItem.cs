using UnityEngine;
using System.Collections;

public class MoveSceneryItem : MonoBehaviour {

    [SerializeField]
    float playerSpeed;
    
    GameController GameControllerScript;


    // Use this for initialization
    void Start () {
        GameControllerScript = GameObject.Find("GameScripts").GetComponent<GameController>();
        playerSpeed = GameControllerScript.playerSpeed;
    }
	
	// Update is called once per frame
	void Update () {
        playerSpeed = GameControllerScript.playerSpeed;
        transform.Translate(0, 0, playerSpeed * Time.deltaTime);

        if (this.tag == "Clouds")
        {
            transform.Translate(1, 0, 0.5f);
        }

        if (transform.position.z >= 5100)
            Destroy(this.gameObject);
	
	}
}
