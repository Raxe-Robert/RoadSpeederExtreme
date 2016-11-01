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
    GameObject scoreMessageObject;
    [SerializeField]
    List<Text> scoreMessagesList;

    [SerializeField]
    GameController GameControllerScript;

    GameObject ScoreMessagesContainer;

	// Use this for initialization
	void Start () {
        GameControllerScript = GameObject.Find("GameScripts").GetComponent<GameController>();
        ScoreMessagesContainer = GameObject.Find("ScoreMessagesContainer");
        scoreMessagesList = new List<Text>();

        for (int i = 0; i < 10; i++)
        {
            var NewMessageObject = Instantiate(scoreMessageObject, scoreMessageObject.transform.position, Quaternion.identity)as GameObject;
            var NewMessageObjectText = NewMessageObject.GetComponent<Text>();

            NewMessageObject.transform.SetParent(ScoreMessagesContainer.transform, false);

            NewMessageObjectText.text = "";
            scoreMessagesList.Add(NewMessageObjectText);

            scoreMessageObject.SetActive(false);
        }

        StartCoroutine(UpdateUiElements());
    }

    public void NewMessage(string text)
    {
        foreach (var messageObject in scoreMessagesList)
        {
            if (messageObject.isActiveAndEnabled == false)
            {
                messageObject.text = text;
                messageObject.gameObject.SetActive(true);
                break;
            }
        }
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
