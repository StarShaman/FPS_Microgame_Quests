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
    [SerializeField] private GameObject QuestBtn, TalkBtn, BackBtn, AcceptBtn, DeclineBtn, EscapeBtn;
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
    public void SetButtonLayout(int layoutType)
    {
        switch (layoutType)
        {
            case 0: // default layout
                QuestBtn.SetActive(true);
                TalkBtn.SetActive(true);
                BackBtn.SetActive(false);
                AcceptBtn.SetActive(false);
                DeclineBtn.SetActive(false);
                EscapeBtn.SetActive(true);
                break;
            case 1: // quest layout (accept/decline)
                QuestBtn.SetActive(false);
                TalkBtn.SetActive(false);
                BackBtn.SetActive(false);
                AcceptBtn.SetActive(true);
                DeclineBtn.SetActive(true);
                EscapeBtn.SetActive(false);
                break;
            case 2: // escape layout
                QuestBtn.SetActive(false);
                TalkBtn.SetActive(false);
                BackBtn.SetActive(false);
                AcceptBtn.SetActive(false);
                DeclineBtn.SetActive(false);
                EscapeBtn.SetActive(true);
                break;
            case 3: // back layout
                QuestBtn.SetActive(false);
                TalkBtn.SetActive(false);
                BackBtn.SetActive(true);
                AcceptBtn.SetActive(false);
                DeclineBtn.SetActive(false);
                EscapeBtn.SetActive(false);
                break;
        }
    }
    public void Show()
    {
        dialogueUIContainer.SetActive(true);
    }

    public void Hide()
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
