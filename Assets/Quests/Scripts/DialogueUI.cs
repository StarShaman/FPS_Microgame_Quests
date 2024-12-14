using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject dialogueUIContainer;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject enterToProceedText;
    private void Start()
    {
        Hide();
    }
    public void SetName(string name)
    {
        nameText.text = name;
    }
    public void SetEnterToProceedVisibility(bool isVisible)
    {
        enterToProceedText.SetActive(isVisible);
    }
    public void SetButtonLayout()
    {
        //
    }
    public void Show()
    {
        dialogueUIContainer.SetActive(true);
    }

    private void Hide()
    {
        dialogueUIContainer.SetActive(false);
        enterToProceedText.SetActive(false);
    }
    public bool IsContainerVisible()
    {
        return dialogueUIContainer.activeSelf;
    }

    public void AddCharacterToText(char c)
    {
        dialogueText.text += c;
    }
    public void SetDialogueText(string dialogueText)
    {
        this.dialogueText.text = dialogueText;
    }
}
