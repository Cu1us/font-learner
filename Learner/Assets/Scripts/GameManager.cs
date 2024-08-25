using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField]
    LearnerPack learnerPack;
    public static LearnerPack Pack { get { return Instance.learnerPack; } }

    public static readonly char[] CharacterFrequencyList = { 'e', 't', 'a', 'o', 'i', 'n', 's', 'h', 'r', 'd', 'l', 'c', 'u', 'm', 'w', 'f', 'g', 'y', 'p', 'b', 'v', 'k', 'j', 'x', 'q', 'z' };

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        SceneManager.LoadScene("LearningScene");
    }
}

public class Glyph
{
    public readonly char Character;
    public uint CorrectAnswers;
    public uint TotalAnswers;
    public float LastAnswerTime;
    private const uint TOTALANSWEROFFSET = 4;
    public double SuccessRatio { get { return TotalAnswers == 0 ? 0 : (double)CorrectAnswers / (TotalAnswers + TOTALANSWEROFFSET); } }
    public void Clear()
    {
        CorrectAnswers = 0;
        TotalAnswers = 0;
        LastAnswerTime = 0;
    }
    public Glyph(char character)
    {
        Character = character;
        Clear();
    }
}