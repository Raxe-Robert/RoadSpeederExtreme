using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIhandler : MonoBehaviour {

    [SerializeField]
    Text playerSpeed;
    [SerializeField]
    Text playerScore;

    [SerializeField]
    Text scoreNotificationObject;
    List<Text> scoreNotifications;

    [SerializeField]
    GameController GameControllerScript;

	// Use this for initialization
	void Start () {
        GameControllerScript = GameObject.Find("GameScripts").GetComponent<GameController>();
        scoreNotifications = new List<Text>();

    }
	
	// Update is called once per frame
	void Update () {
        playerSpeed.text = "Speed: " + GameControllerScript.playerSpeed.ToString();
        playerScore.text = "Score: " + GameControllerScript.playerScore.ToString();

        foreach(var text in scoreNotifications)
        {
            var pos = text.rectTransform.anchoredPosition;
            pos.y += 2;
            text.rectTransform.anchoredPosition = pos;

            if (text.rectTransform.anchoredPosition.y >= Screen.height)
            {
                Destroy(text);
            }
        }
    }

    public void NewMessage(string text)
    {
        
        var newNotificationText = Instantiate(scoreNotificationObject);
        newNotificationText.transform.SetParent(this.gameObject.transform);

        var pos = newNotificationText.rectTransform.anchoredPosition;
        pos.y = -Screen.height / 4;
        pos.x = 0;
        newNotificationText.rectTransform.anchoredPosition = pos;

        newNotificationText.text = "+" + text;
        scoreNotifications.Add(newNotificationText);
    }


}
