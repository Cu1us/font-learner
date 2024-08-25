using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LearningManager : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI CharacterDisplayText;
    [SerializeField]
    TextMeshProUGUI ProgressLabel;
    [SerializeField]
    TextMeshProUGUI AverageScoreLabel;

    [SerializeField]
    AnswerButton SingleAnswerButton;

    [SerializeField]
    GameObject MultiAnswerFrame;
    [SerializeField]
    AnswerButton[] MultiAnswerButtons;

    char? currentChar = null;
    char? lastChar = null;
    bool FailedThisGlyph = false;
    readonly Dictionary<char, Glyph> LearningChars = new();

    /// <summary>
    /// If there is no pending glyph above this weight, the next glyph shown will be a new one.
    /// </summary>
    public double BaseNewCharacterWeight;
    /// <summary>
    /// If a glyph's success ratio is below this, show the correct answer
    /// </summary>
    public double MinimumSuccessRatioForSingleButton;
    /// <summary>
    /// If the average success ratio is above this, new characters appear more frequently
    /// </summary>
    public double AverageSuccessRatioForIncreasedNewGlyphs;

    private void Start()
    {
        CharacterDisplayText.font = GameManager.Pack.Font;
        CharacterDisplayText.rectTransform.anchoredPosition = new Vector2(GameManager.Pack.FontOffsetX, CharacterDisplayText.rectTransform.anchoredPosition.y);
        DisplayNextCharacter();
    }
    public void AnswerButtonClicked(char clickedCharacter, AnswerButton sourceButton)
    {
        LearningChars[currentChar.Value].TotalAnswers++;
        if (clickedCharacter == currentChar)
        {
            Debug.Log("CORRECT! " + currentChar);
            if (!FailedThisGlyph) LearningChars[currentChar.Value].CorrectAnswers++;
            LearningChars[currentChar.Value].LastAnswerTime = Time.time;
            lastChar = currentChar;
            DisplayNextCharacter();
        }
        else
        {
            Debug.Log("WRONG! " + currentChar);
            FailedThisGlyph = true;
            sourceButton.Interactable = false;
        }
    }

    [ContextMenu("Next character")]
    public void DisplayNextCharacter()
    {
        currentChar = GetNextChar();
        FailedThisGlyph = false;
        // TODO: Null check here in case there's nothing more to learn
        CharacterDisplayText.text = currentChar.ToString();
        Glyph currentGlyph = LearningChars[currentChar.Value];
        if (currentGlyph.CorrectAnswers < 1 || currentGlyph.SuccessRatio < MinimumSuccessRatioForSingleButton)
        {
            MultiAnswerFrame.SetActive(false);
            SingleAnswerButton.gameObject.SetActive(true);
            SingleAnswerButton.SetCharacter(currentChar.Value);
        }
        else
        {
            SingleAnswerButton.gameObject.SetActive(false);

            int correctButton = Random.Range(0, MultiAnswerButtons.Length);
            List<char> ExhaustiveCharList = new(GameManager.Pack.CharacterList);
            ExhaustiveCharList.Remove(currentChar.Value);
            for (int i = 0; i < MultiAnswerButtons.Length; i++)
            {
                if (i == correctButton)
                {
                    MultiAnswerButtons[i].SetCharacter(currentChar.Value);
                }
                else
                {
                    char buttonChar = ExhaustiveCharList[Random.Range(0, ExhaustiveCharList.Count)];
                    MultiAnswerButtons[i].SetCharacter(buttonChar);
                    ExhaustiveCharList.Remove(buttonChar);
                }
                MultiAnswerButtons[i].Interactable = true;
            }

            MultiAnswerFrame.SetActive(true);
        }
    }
    public char? GetNextChar() // Returns null if there are no more characters to learn
    {
        // Get glyph with highest weight, and calculate average success ratio
        double highestWeight = 0;
        Glyph highestWeightedGlyph = null;
        double averageSuccessRatio = 0;
        bool allGlyphsAboveAverageRequirement = true;
        foreach (KeyValuePair<char, Glyph> charGlyphPair in LearningChars)
        {
            double weight = GetGlyphWeight(charGlyphPair.Value);
            if (weight > highestWeight)
            {
                highestWeightedGlyph = charGlyphPair.Value;
                highestWeight = weight;
            }
            averageSuccessRatio += charGlyphPair.Value.SuccessRatio;
            if (allGlyphsAboveAverageRequirement && charGlyphPair.Value.SuccessRatio < AverageSuccessRatioForIncreasedNewGlyphs) allGlyphsAboveAverageRequirement = false;
        }

        // Calculate the weight of new characters
        averageSuccessRatio /= LearningChars.Count;
        AverageScoreLabel.text = $"Average Score\n{System.Math.Round(averageSuccessRatio * 100)}%";
        ProgressLabel.text = $"{LearningChars.Count}/{GameManager.Pack.CharacterList.Length}";
        double newCharacterWeight = BaseNewCharacterWeight;
        if (averageSuccessRatio > AverageSuccessRatioForIncreasedNewGlyphs && allGlyphsAboveAverageRequirement)
        {
            double knowledgeFraction = averageSuccessRatio - AverageSuccessRatioForIncreasedNewGlyphs;
            knowledgeFraction /= 1 - AverageSuccessRatioForIncreasedNewGlyphs;
            // The closer all glyphs are to 100% success ratio, the more the new glyph weight skyrockets
            newCharacterWeight /= 1 / knowledgeFraction;
            newCharacterWeight += 1;
        }

        // Determine the glyph to show
        if (highestWeight > newCharacterWeight)
        {
            Debug.Log($"Highest glyph weight ({highestWeightedGlyph.Character}): {highestWeight}");
            return highestWeightedGlyph.Character;
        }
        else if (TryGetNextUnlearnedCharacter(out char nextUnlearnedChar))
        {
            Debug.Log("Getting new glyph!");
            LearningChars[nextUnlearnedChar] = new Glyph(nextUnlearnedChar);
            return nextUnlearnedChar;
        }
        else return null;
    }
    double GetGlyphWeight(Glyph glyph)
    {
        if (glyph.Character == lastChar) return 0;
        double weight = 1 / System.Math.Pow(glyph.SuccessRatio, 2);
        float timeSinceLastAnswer = Time.time - glyph.LastAnswerTime;
        weight *= Mathf.Log(timeSinceLastAnswer * timeSinceLastAnswer + 1, 2.71f) / 3;
        if (glyph.CorrectAnswers < 5) weight += 10 - 2 * glyph.CorrectAnswers;
        return weight;
    }
    bool TryGetNextUnlearnedCharacter(out char nextChar)
    {
        foreach (char character in GameManager.Pack.CharacterList)
        {
            if (LearningChars.ContainsKey(character)) continue;
            nextChar = character;
            return true;
        }
        nextChar = char.MinValue;
        return false;
    }
}