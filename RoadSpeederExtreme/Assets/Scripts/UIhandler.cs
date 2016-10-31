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
    Text scoreNotificationObject;
    List<Text> scoreNotifications;

    [SerializeField]
    GameController GameControllerScript;

	// Use this for initialization
	void Start () {
        GameControllerScript = GameObject.Find("GameScripts").GetComponent<GameController>();
        scoreNotifications = new List<Text>();

        StartCoroutine(UpdateUiText());
    }
	
	// Update is called once per frame
	void Update () {
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

    IEnumerator UpdateUiText()
    {
        while (true)
        {
            playerSpeed.text = "Speed: " + GameControllerScript.playerSpeed.ToString();
            playerScore.text = "Score: " + GameControllerScript.playerScore.ToString();

            gameTimer.text = (GameControllerScript.timerSeconds < 10 ? 
                GameControllerScript.timerMinutes + ":0" + GameControllerScript.timerSeconds : 
                GameControllerScript.timerMinutes + ":" + GameControllerScript.timerSeconds);

            countdownTimer.text = (GameControllerScript.unpauseCountdown <= 0 ?
                "" :
                "" + GameControllerScript.unpauseCountdown);

            yield return new WaitForSecondsRealtime(0.1f);
        }
    }
}
