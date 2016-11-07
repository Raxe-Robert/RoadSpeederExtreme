import UnityEngine;
import System.Collections;
import UnityEngine.UI;
import System.Collections.Generic;

public class UIhandler extends MonoBehaviour {

    @SerializeField
    var playerSpeed: Text;
    @SerializeField
    var playerScore: Text;
    @SerializeField
    var actualScore: Text;    
    @SerializeField
    var gameTimer: Text;
    @SerializeField
    var countdownTimer: Text;

    @SerializeField
    var speedPointer: Image;

    @SerializeField
    var scoreMessageObject: GameObject;
    private var pool_ScoreMessages: GameObject;
    @SerializeField
    var scoreMessagesList: List.<Text>;

    @SerializeField
    var GameControllerScript: GameController;


	// Use this for initialization
	private function Start() {
        GameControllerScript = GameObject.Find("GameScripts").GetComponent.<GameController>();
        pool_ScoreMessages = GameObject.Find("pool_ScoreMessages");
        scoreMessagesList = new List.<Text>();

        for (var i: int = 0; i < 10; i++)
        {
            var NewMessageObject = Instantiate(scoreMessageObject, scoreMessageObject.transform.position, Quaternion.identity)as GameObject;
            var NewMessageObjectText = NewMessageObject.GetComponent.<Text>();

            NewMessageObject.transform.SetParent(pool_ScoreMessages.transform, false);

            NewMessageObjectText.text = "";
            scoreMessagesList.Add(NewMessageObjectText);

            NewMessageObject.SetActive(false);
        }

        StartCoroutine(UpdateUiElements());
    }

    public function NewMessage(text: String)
    {
        for (var messageObject: in scoreMessagesList)
        {
            if (messageObject.isActiveAndEnabled == false)
            {
                messageObject.text = text;
                messageObject.gameObject.SetActive(true);
                break;
            }
        }
    }

    private function UpdateUiElements(): IEnumerator
    {
        while (true)
        {
            playerSpeed.text = "" + GameControllerScript.playerSpeed / 2;
            playerScore.text = "" + GameControllerScript.playerScore;
            actualScore.text = "" + playerScore.text;

            gameTimer.text = (GameControllerScript.timerSeconds < 10 ? 
                GameControllerScript.timerMinutes + ":0" + GameControllerScript.timerSeconds : 
                GameControllerScript.timerMinutes + ":" + GameControllerScript.timerSeconds);

            countdownTimer.text = (GameControllerScript.unpauseCountdown <= 0 ?
                "" :
                "" + GameControllerScript.unpauseCountdown);

            var speedPercentage: float = 0;
            if (GameControllerScript.playerSpeed >= 0 || GameControllerScript.playerSpeed <= GameControllerScript.maxPlayerSpeed)
            {
                speedPercentage = parseFloat(GameControllerScript.playerSpeed) / GameControllerScript.maxPlayerSpeed * 100;

                speedPointer.rectTransform.rotation = Quaternion.Euler(0, 0, 90 - speedPercentage * 1.8);
            }

            yield  WaitForSecondsRealtime(0.1);
        }
    }
}
