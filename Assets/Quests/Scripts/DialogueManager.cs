using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    [SerializeField] private DialogueUI dialogueUI;

    private int currentDialogueIndex = 0;
    private bool enterToProceedTextVisible = true;
    private bool isQuestChoiceDialogue = false;
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
            // Ask about the quest button
            if (currentDialogueIndex == 0 && !isQuestChoiceDialogue)
            {
                dialogueUI.SetButtonLayout(0);
                if (QuestManager.Instance.GetQuestStatusByCode(currentQuestData.questId) == QuestStatus.InProgress)
                {
                    currentDialogueLines = new string[] { currentQuestData.questOngoing };
                }
                else if (QuestManager.Instance.GetQuestStatusByCode(currentQuestData.questId) == QuestStatus.Completed)
                {
                    currentDialogueLines = new string[] { currentQuestData.questCompleted };
                    QuestManager.Instance.ClaimQuest(currentQuestData);
                    // NPC will give out rewards here
                }
                else if (QuestManager.Instance.GetQuestStatusByCode(currentQuestData.questId) == QuestStatus.Failed)
                {
                    currentDialogueLines = new string[] { currentQuestData.questFailed };
                }
                else if (QuestManager.Instance.GetQuestStatusByCode(currentQuestData.questId) == QuestStatus.CompletedAndClaimed)
                {
                    currentDialogueLines = new string[] { currentQuestData.questItemAlreadyObtainedLine };
                }
                else // Quest not started
                {
                    currentDialogueLines = currentQuestData.questDialogueLines;
                }
                StartCoroutine(ShowDialogue());
            }
            else if (IsLastDialogueLine() && IsQuestDialogue()) // Accept the quest button
            {
                AcceptQuest();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // Talk with them button
            if (currentDialogueIndex == 0 && !isQuestChoiceDialogue)
            {
                currentDialogueLines = currentQuestData.dialogueLines;
                StartCoroutine(ShowDialogue());
            }
            else if (IsLastDialogueLine() && IsQuestDialogue()) // Decline the quest button
            {
                DeclineQuest();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (IsLastDialogueLine() || currentDialogueIndex == 0)
            {
                if (IsQuestDialogue() && currentDialogueIndex != 0)
                    return;
                // Exit the dialogue button
                dialogueUI.Hide();
            }
            else // Back button to go to the previous dialogue line 
            {
                ShowPreviousDialogue();
            }
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (enterToProceedTextVisible)
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
        isQuestChoiceDialogue = false;
        currentQuestData = questData;
        dialogueUI.SetButtonLayout(0);
        dialogueUI.SetName(npcName);
        this.npcName = npcName;
        dialogueUI.Show();

        currentDialogueIndex = 0;

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
    }
    private void ShowNextDialogue()
    {
        if (currentDialogueIndex < currentDialogueLines.Length - 1)
        {
            currentDialogueIndex++;
            ButtonUISwitcher();
            StartCoroutine(ShowDialogue());
        }
    }
    private void ShowPreviousDialogue()
    {
        if (currentDialogueIndex > 0 && currentDialogueIndex < currentDialogueLines.Length)
        {
            currentDialogueIndex--;
            ButtonUISwitcher();
            StartCoroutine(ShowDialogue());
        }
    }
    // Checking if the last dialogue line has been reached
    private bool IsLastDialogueLine()
    {
        return currentDialogueIndex == currentDialogueLines.Length - 1;
    }
    // Checking if the current dialogue line is not the first and last
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
        isQuestChoiceDialogue = true;
        dialogueUI.SetButtonLayout(2);
        currentDialogueIndex = 0;
        currentDialogueLines = new string[] { currentQuestData.questAccepted };
        QuestManager.Instance.StartQuest(currentQuestData);
        StartCoroutine(ShowDialogue());
    }
    public void DeclineQuest()
    {
        isQuestChoiceDialogue = true;
        dialogueUI.SetButtonLayout(2);
        currentDialogueIndex = 0;
        currentDialogueLines = new string[] { currentQuestData.questDeclined };
        StartCoroutine(ShowDialogue());
    }
}
