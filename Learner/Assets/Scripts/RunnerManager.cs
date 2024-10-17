using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RunnerManager : MonoBehaviour
{
    [SerializeField]
    RunnerPlayer player;
    [SerializeField]
    RunnerWall wallPrefab;
    [SerializeField]
    Transform wallContainer;

    [SerializeField]
    TextMeshProUGUI topText;
    [SerializeField]
    TextMeshProUGUI scoreText;

    Vector2 screenTouchPoint;

    bool correctAnswerIsLeft;

    int score;
    float timeToAnswer = 4f;

    void Start()
    {
        screenTouchPoint = Camera.main.ViewportToScreenPoint(new Vector2(0.5f, 0.5f));
    }

    void Update()
    {
        UpdatePlayerPosition();
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SpawnWall();
        }
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

    void SpawnWall()
    {
        RunnerWall wall = Instantiate(wallPrefab, wallContainer);
        wall.fallDuration = timeToAnswer;
        wall.hitCallback += OnWallHit;

        string correctWord = GetRandomWord();
        string fakeWord = GetRandomWord();

        correctAnswerIsLeft = Random.Range(0, 2) == 0; // 50%

        if (correctAnswerIsLeft)
        {
            wall.SetText(correctWord, fakeWord);
        }
        else
        {
            wall.SetText(fakeWord, correctWord);
        }
        topText.text = correctWord;
    }
    void OnWallHit(RunnerWall wall)
    {
        bool success = player.currentX < 0.5f && correctAnswerIsLeft;
        if (success)
        {
            score++;
            scoreText.text = score.ToString();
            timeToAnswer *= 0.9f;
        }
        else
        {
            if (timeToAnswer < 4f)
            {
                timeToAnswer = Mathf.Min(timeToAnswer + 0.25f, 4f);
            }
            Destroy(wall.gameObject);
        }
    }
    string GetRandomWord(int minLength = 0, int maxLength = int.MaxValue)
    {
        return new string[] { "abc", "def", "ghi", "jkl", "mno", "pqr", "stu", "vxy" }[Random.Range(0, 3)];
    }
}
