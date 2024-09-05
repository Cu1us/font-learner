using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerManager : MonoBehaviour
{
    [SerializeField]
    RunnerPlayer player;

    Vector2 screenTouchPoint;

    void Start()
    {
        screenTouchPoint = Camera.main.ViewportToScreenPoint(new Vector2(0.5f, 0.5f));
    }

    void Update()
    {
        UpdatePlayerPosition();
    }

    void UpdatePlayerPosition()
    {
        if (Input.touchCount > 0)
        {
            screenTouchPoint = Input.GetTouch(0).position;
        }
        else if (Input.mousePresent)
        {
            screenTouchPoint = Input.mousePosition;
        }
        float playerX = Camera.main.ScreenToViewportPoint(screenTouchPoint).x;
        player.SetPositionX(playerX);
    }
}
