using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[RequireComponent(typeof(RectTransform))]
public class RunnerWall : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI LeftLabel;
    [SerializeField]
    TextMeshProUGUI RightLabel;

    RectTransform rectTransform;

    public float fallDuration;
    public float targetY;
    float startY;

    public bool hasHit = false;

    float fallProgress = 0;

    public Action<RunnerWall> hitCallback;

    public void SetText(string left, string right)
    {
        LeftLabel.text = left;
        RightLabel.text = right;
    }
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startY = Camera.main.ViewportToScreenPoint(new Vector2(0.5f, 1f)).y;
        UpdatePosition();
    }
    private void Update()
    {
        fallProgress += 1 / fallDuration * Time.deltaTime;
        UpdatePosition();
        if (!hasHit && fallProgress >= 1)
        {
            hasHit = true;
            hitCallback?.Invoke(this);
        }
    }
    void UpdatePosition()
    {
        float newY = Mathf.LerpUnclamped(startY, targetY, fallProgress);
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, newY);
    }
}
