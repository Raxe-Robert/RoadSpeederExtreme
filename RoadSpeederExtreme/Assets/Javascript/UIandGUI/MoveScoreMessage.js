import UnityEngine;
import System.Collections;

public class MoveScoreMessage extends MonoBehaviour {
    
    private var rectTransformCurrent: RectTransform;

    private var yDirectionSpeed: float;
    private var xDirectionSpeed: float;

    private var lifetimeValue: float = 1;
    private var lifetime: float;

    private var moveSpeed: float = 10;

    private var setEnablePosition: boolean;

    private function OnEnable()
    {
        setEnablePosition = true;

        yDirectionSpeed = Random.Range(0, moveSpeed + 1);
        xDirectionSpeed = moveSpeed - yDirectionSpeed;

        yDirectionSpeed = Random.Range(0, 2) == 0 ? yDirectionSpeed * -1 : yDirectionSpeed;
        xDirectionSpeed = Random.Range(0, 2) == 0 ? xDirectionSpeed * -1 : xDirectionSpeed;

        lifetime = lifetimeValue;
    }

	// Use this for initialization
	private function Start() {
        rectTransformCurrent = GetComponent.<RectTransform>();

        setEnablePosition = true;
    }

    private function Update()
    {
        if(setEnablePosition)
            WarpText();
        
        if (lifetime >= 0)
        {
            var pos = rectTransformCurrent.anchoredPosition;
            pos.y += yDirectionSpeed * moveSpeed * Time.deltaTime;
            pos.x += xDirectionSpeed * moveSpeed * Time.deltaTime;
            rectTransformCurrent.anchoredPosition = pos;

            lifetime -= Time.deltaTime;
        }
        else
        {
            var pos = rectTransformCurrent.anchoredPosition;
            pos.y = 0;
            pos.x = 0;
            rectTransformCurrent.anchoredPosition = pos;

            this.gameObject.SetActive(false);
        }
    }

    private function WarpText()
    {
        //Set the pos further so no overlapping occurs with playerScore
        var pos = rectTransformCurrent.anchoredPosition;
        pos.y += yDirectionSpeed * 6;
        pos.x += xDirectionSpeed * 6;
        rectTransformCurrent.anchoredPosition = pos;

        setEnablePosition = false;
    }
}
