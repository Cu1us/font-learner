using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class AnswerButton : MonoBehaviour
{
    [HideInInspector]
    public Button button;

    public TextMeshProUGUI ButtonLabel;
    public LearningManager learningManager;

    public char Character;

    public string Text { get { return ButtonLabel.text; } set { ButtonLabel.text = value; } }
    public bool Interactable { get { return button.interactable; } set { button.interactable = value; } }

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }
    public void SetCharacter(char character)
    {
        Character = character;
        ButtonLabel.text = character.ToString();
    }
    public void SetCharacter(char? character)
    {
        Character = character.Value;
        ButtonLabel.text = character == null ? "?" : character.ToString();
    }
    void OnClick()
    {
        if (learningManager)
        {
            learningManager.AnswerButtonClicked(Character, this);
        }
    }
}
