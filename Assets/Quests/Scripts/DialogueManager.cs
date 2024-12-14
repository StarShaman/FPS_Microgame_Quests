using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    [SerializeField] private DialogueUI dialogueUI;

    private int currentDialogueIndex = 0;
    private bool enterToProceedVisible = true;
    private bool isADialogueLineComplete = false;
    private string[] currentDialogueLines;
    private string npcName;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        if (dialogueUI.IsContainerVisible())
        {
            HandleDialogueInput();
        }
    }

    private void HandleDialogueInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // Ask about the quest OR accept the quest
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // Decline the quest OR decline the quest
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            // Back button to go to the previous dialogue line OR exit the dialogue
            ShowPreviousDialogue();
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (enterToProceedVisible && isADialogueLineComplete)
            {
                ShowNextDialogue();
            }
        }
    }
    private void QuestButtonActivate()
    {
        
    }
    public void StartDialogue(string npcName, QuestsAndDialoguesSO questData)
    {
        dialogueUI.SetName(npcName);
        this.npcName = npcName;
        dialogueUI.Show();

        currentDialogueIndex = 0;
        isADialogueLineComplete = false;

        currentDialogueLines = questData.dialogueLines;
        SetDialogueSet();

        StartCoroutine(ShowDialogue());
    }
    private void SetDialogueSet()
    {

    }
    private string HandlePlayerTagInText(string line)
    {
        if (line.EndsWith("[P]"))
        {
            // will change portrait later too
            dialogueUI.SetName("Player");
            line = line.Replace("[P]", "");
            return line;
        }
        else
        {
            dialogueUI.SetName(npcName);
            return line;
        }
    }
    private IEnumerator ShowDialogue()
    {
        string dialogueLine = currentDialogueLines[currentDialogueIndex];
        dialogueLine = HandlePlayerTagInText(dialogueLine);
        dialogueUI.SetDialogueText("");

        foreach (char c in dialogueLine)
        {
            dialogueUI.AddCharacterToText(c); // Adding one character at a time
            yield return new WaitForSeconds(0.01f);
            // break out of the loop if the player has clicked E to reset the dialogue
        }

        isADialogueLineComplete = true;
    }
    private void ShowNextDialogue()
    {
        if (currentDialogueIndex < currentDialogueLines.Length - 1)
        {
            currentDialogueIndex++;
            StartCoroutine(ShowDialogue());
        }
    }
    private void ShowPreviousDialogue()
    {
        if (currentDialogueIndex > 0 && currentDialogueIndex < currentDialogueLines.Length)
        {
            currentDialogueIndex--;
            StartCoroutine(ShowDialogue());
        }
    }
}
