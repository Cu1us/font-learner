using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class RunnerPlayer : MonoBehaviour
{
    RectTransform rectTransform;
    public float currentX = 0.5f;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public void SetPositionX(float x)
    {
        currentX = x;
        rectTransform.anchorMin = new Vector2(currentX, 0);
        rectTransform.anchorMax = new Vector2(currentX, 0);
    }
}