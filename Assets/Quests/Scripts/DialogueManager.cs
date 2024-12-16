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
    private QuestsAndDialoguesSO currentQuestData = null;

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
            HandleEnterToProceedText();
        }
    }

    private void ButtonUISwitcher()
    {
        if (IsNotFirstOrLastDialogueLine())
        {
            // Show ONLY the back button
            dialogueUI.SetButtonLayout(3);
        }
        else if (IsLastDialogueLine() && IsQuestDialogue())
        {
            // Show ONLY the accept/decline buttons
            dialogueUI.SetButtonLayout(1);
        }
        else if (IsLastDialogueLine())
        {
            // Show ONLY the escape button
            dialogueUI.SetButtonLayout(2);
        }
        else
        {
            // Show the default buttons
            dialogueUI.SetButtonLayout(0);
        }
    }

    private void HandleDialogueInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // Ask about the quest OR accept the quest
            if (currentDialogueIndex == 0)
            {
                dialogueUI.SetButtonLayout(0);
                currentDialogueLines = currentQuestData.questDialogueLines;
                StartCoroutine(ShowDialogue());
            }
            else if (IsLastDialogueLine() && IsQuestDialogue())
            {
                AcceptQuest();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // Talk with them OR decline the quest
            if (currentDialogueIndex == 0)
            {
                currentDialogueLines = currentQuestData.dialogueLines;
                StartCoroutine(ShowDialogue());
            }
            else if (IsLastDialogueLine() && IsQuestDialogue())
            {
                DeclineQuest();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            // Back button to go to the previous dialogue line OR exit the dialogue
            if (IsLastDialogueLine())
            {
                if (IsQuestDialogue())
                    return;
                // Exit the dialogue
                dialogueUI.Hide();
            }
            else
            {
                ShowPreviousDialogue();
            }
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (enterToProceedVisible && isADialogueLineComplete)
            {
                ShowNextDialogue();
            }
        }
    }


    private void HandleEnterToProceedText()
    {
        // fix constantly setting the visibility of the enter to proceed text
        if (IsLastDialogueLine())
        {
            dialogueUI.SetEnterToProceedVisibility(false);
        }
        else
        {
            dialogueUI.SetEnterToProceedVisibility(true);
        }
    }
    public void StartDialogue(string npcName, QuestsAndDialoguesSO questData)
    {
        currentQuestData = questData;
        dialogueUI.SetButtonLayout(0);
        dialogueUI.SetName(npcName);
        this.npcName = npcName;
        dialogueUI.Show();

        currentDialogueIndex = 0;
        isADialogueLineComplete = false;

        currentDialogueLines = questData.dialogueLines;

        StartCoroutine(ShowDialogue());
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
            ButtonUISwitcher();
            currentDialogueIndex++;
            StartCoroutine(ShowDialogue());
        }
    }
    private void ShowPreviousDialogue()
    {
        if (currentDialogueIndex > 0 && currentDialogueIndex < currentDialogueLines.Length)
        {
            ButtonUISwitcher();
            currentDialogueIndex--;
            StartCoroutine(ShowDialogue());
        }
    }
    // Checking if the last dialogue line has been reached
    private bool IsLastDialogueLine()
    {
        return currentDialogueIndex == currentDialogueLines.Length - 1;
    }
    // Check whether the current dialogue is the second line
    private bool IsSecondDialogueLine()
    {
        return currentDialogueIndex == 1;
    }
    private bool IsNotFirstOrLastDialogueLine()
    {
        return currentDialogueIndex != 0 && currentDialogueIndex != currentDialogueLines.Length - 1;
    }
    // Check whether the current dialogue is a quest dialogue
    private bool IsQuestDialogue()
    {
        return currentDialogueLines == currentQuestData.questDialogueLines;
    }
    // Button controls
    public void AcceptQuest()
    {
        dialogueUI.SetButtonLayout(2);
        currentDialogueLines = new String[] { currentQuestData.questAccepted };
        StartCoroutine(ShowDialogue());
    }
    public void DeclineQuest()
    {
        dialogueUI.SetButtonLayout(2);
        currentDialogueLines = new String[] { currentQuestData.questDeclined };
        StartCoroutine(ShowDialogue());
    }
}
