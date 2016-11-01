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
    Text gameTimer;
    [SerializeField]
    Text countdownTimer;

    [SerializeField]
    Image speedPointer;

    [SerializeField]
    Text scoreNotificationObject;
    List<Text> scoreNotifications;

    [SerializeField]
    GameController GameControllerScript;

	// Use this for initialization
	void Start () {
        GameControllerScript = GameObject.Find("GameScripts").GetComponent<GameController>();
        scoreNotifications = new List<Text>();

        StartCoroutine(UpdateUiElements());
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Move score notifications up and delete them at x
        for (int i = scoreNotifications.Count -1; i >= 0; i--)
        {
            var text = scoreNotifications[i];
            var pos = text.rectTransform.anchoredPosition;
            pos.y += 2;
            text.rectTransform.anchoredPosition = pos;

            if (text.rectTransform.anchoredPosition.y >= -44)
            {
                Destroy(text.gameObject);
                scoreNotifications.RemoveAt(i);
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

    IEnumerator UpdateUiElements()
    {
        while (true)
        {
            playerSpeed.text = "" + GameControllerScript.playerSpeed;
            playerScore.text = "" + GameControllerScript.playerScore;

            gameTimer.text = (GameControllerScript.timerSeconds < 10 ? 
                GameControllerScript.timerMinutes + ":0" + GameControllerScript.timerSeconds : 
                GameControllerScript.timerMinutes + ":" + GameControllerScript.timerSeconds);

            countdownTimer.text = (GameControllerScript.unpauseCountdown <= 0 ?
                "" :
                "" + GameControllerScript.unpauseCountdown);

            float speedPercentage = 0;
            if (GameControllerScript.playerSpeed >= 0 || GameControllerScript.playerSpeed <= GameControllerScript.maxPlayerSpeed)
            {
                speedPercentage = (float)GameControllerScript.playerSpeed / (float)GameControllerScript.maxPlayerSpeed * 100f;

                speedPointer.rectTransform.rotation = Quaternion.Euler(0, 0, 90 - speedPercentage * 1.8f);
            }

            yield return new WaitForSecondsRealtime(0.1f);
        }
    }
}
