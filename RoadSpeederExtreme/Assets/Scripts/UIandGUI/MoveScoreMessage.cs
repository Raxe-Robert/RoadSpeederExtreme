using UnityEngine;
using System.Collections;

public class MoveScoreMessage : MonoBehaviour {
    
    RectTransform rectTransformCurrent;

    float yDirectionSpeed;
    float xDirectionSpeed;

    float lifetimeValue = 1f;
    float lifetime;

    float moveSpeed = 10;

    bool setEnablePosition;

    void OnEnable()
    {
        setEnablePosition = true;

        yDirectionSpeed = Random.Range(0, moveSpeed + 1);
        xDirectionSpeed = moveSpeed - yDirectionSpeed;

        yDirectionSpeed = Random.Range(0, 2) == 0 ? yDirectionSpeed * -1 : yDirectionSpeed;
        xDirectionSpeed = Random.Range(0, 2) == 0 ? xDirectionSpeed * -1 : xDirectionSpeed;

        lifetime = lifetimeValue;
    }

	// Use this for initialization
	void Start () {
        rectTransformCurrent = GetComponent<RectTransform>();

        setEnablePosition = true;
    }

    void Update ()
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

    void WarpText()
    {
        //Set the pos further so no overlapping occurs with playerScore
        var pos = rectTransformCurrent.anchoredPosition;
        pos.y += yDirectionSpeed * 6;
        pos.x += xDirectionSpeed * 6;
        rectTransformCurrent.anchoredPosition = pos;

        setEnablePosition = false;
    }
}
