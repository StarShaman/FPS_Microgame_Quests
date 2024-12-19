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
    private Coroutine currentCoroutine;
    private bool isCoroutineRunning = false;

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
    private void Start()
    {
        QuestManager.Instance.OnMainQuestCantStartDueToPreviousQuestNotCompleted += QuestManager_OnMainQuestCantStartDueToPreviousQuestNotCompleted;
    }

    private void QuestManager_OnMainQuestCantStartDueToPreviousQuestNotCompleted(object sender, QuestsAndDialoguesSO quest)
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentDialogueLines = new string[] { "You cannot start this quest yet. Look for other quests to complete." };
        Debug.Log("You cannot start this quest yet. Look for other quests to complete.");
        currentCoroutine = StartCoroutine(ShowDialogue());
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
        Alpha1nput();
        Alpha2Input();
        Alpha3Input();
        EnterInput();
    }

    private void EnterInput()
    {
        if (Input.GetKeyDown(KeyCode.Return) && enterToProceedTextVisible)
        {
            ShowNextDialogue();
        }
    }

    private void Alpha3Input()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (IsLastDialogueLine() || currentDialogueIndex == 0)
            {
                if (!IsQuestDialogue())
                {
                    dialogueUI.Hide();
                }
            }
            else // Back button to go to the previous dialogue line 
            {
                ShowPreviousDialogue();
            }
        }
    }

    private void Alpha2Input()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // Talk with them button
            if (currentDialogueIndex == 0 && !isQuestChoiceDialogue)
            {
                currentDialogueLines = currentQuestData.dialogueLines;
                currentCoroutine = StartCoroutine(ShowDialogue());
            }
            else if (IsLastDialogueLine() && IsQuestDialogue()) // Decline the quest button
            {
                HandleQuestChoice(false);
            }
        }
    }

    private void Alpha1nput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // Ask about the quest button
            if (currentDialogueIndex == 0 && !isQuestChoiceDialogue)
            {
                dialogueUI.SetButtonLayout(0);
                switch (QuestManager.Instance.GetQuestStatusByCode(currentQuestData.questId))
                {
                    case QuestStatus.InProgress:
                        currentDialogueLines = new[] { currentQuestData.questOngoing };
                        break;
                    case QuestStatus.Completed:
                        currentDialogueLines = new[] { currentQuestData.questCompleted };
                        QuestManager.Instance.ClaimQuest(currentQuestData);
                        break;
                    case QuestStatus.Failed:
                        currentDialogueLines = new[] { currentQuestData.questFailed };
                        break;
                    case QuestStatus.CompletedAndClaimed:
                        currentDialogueLines = new[] { currentQuestData.questItemAlreadyObtainedLine };
                        break;
                    default:
                        currentDialogueLines = currentQuestData.questDialogueLines;
                        break;
                }
                currentCoroutine = StartCoroutine(ShowDialogue());
            }
            else if (IsLastDialogueLine() && IsQuestDialogue()) // Accept the quest button
            {
                HandleQuestChoice(true);
            }
        }
    }

    private void HandleEnterToProceedText()
    {
        dialogueUI.SetEnterToProceedVisibility(!IsLastDialogueLine());
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

        currentCoroutine = StartCoroutine(ShowDialogue());
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
        isCoroutineRunning = true;
        string dialogueLine = currentDialogueLines[currentDialogueIndex];
        dialogueLine = HandlePlayerTagInText(dialogueLine);
        dialogueUI.SetDialogueText("");

        foreach (char c in dialogueLine)
        {
            dialogueUI.AddCharacterToText(c);
            yield return new WaitForSeconds(0.01f);
        }

        currentCoroutine = null;
        isCoroutineRunning = false;
    }
    private void ShowNextDialogue()
    {
        AutoFill();
        if (currentDialogueIndex < currentDialogueLines.Length - 1 && !isCoroutineRunning)
        {
            currentDialogueIndex++;
            ButtonUISwitcher();
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = StartCoroutine(ShowDialogue());
        }
    }

    private void ShowPreviousDialogue()
    {
        AutoFill();
        if (currentDialogueIndex > 0 && currentDialogueIndex < currentDialogueLines.Length
            && !isCoroutineRunning)
        {
            currentDialogueIndex--;
            ButtonUISwitcher();
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = StartCoroutine(ShowDialogue());
        }
    }
    private void AutoFill()
    {
        if (isCoroutineRunning)
        {
            StopCoroutine(currentCoroutine);
            dialogueUI.SetDialogueText(currentDialogueLines[currentDialogueIndex]);
            isCoroutineRunning = false;
        }
    }
    private bool IsLastDialogueLine()
    {
        return currentDialogueIndex == currentDialogueLines.Length - 1;
    }
    private bool IsNotFirstOrLastDialogueLine()
    {
        return currentDialogueIndex != 0 && currentDialogueIndex != currentDialogueLines.Length - 1;
    }
    private bool IsQuestDialogue()
    {
        return currentDialogueLines == currentQuestData.questDialogueLines;
    }
    // Button controls
    public void HandleQuestChoice(bool acceptQuest)
    {
        isQuestChoiceDialogue = true;
        dialogueUI.SetButtonLayout(2);
        currentDialogueIndex = 0;

        currentDialogueLines = new string[] { acceptQuest ? currentQuestData.questAccepted : currentQuestData.questDeclined };

        if (acceptQuest)
        {
            QuestManager.Instance.StartQuest(currentQuestData);
        }

        StartCoroutine(ShowDialogue());
    }
}
