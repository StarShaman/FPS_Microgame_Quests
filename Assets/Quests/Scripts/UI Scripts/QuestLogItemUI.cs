using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestLogItemUI : MonoBehaviour
{
    public QuestsAndDialoguesSO questData { get; private set; }
    private QuestStatus questStatus;
    [SerializeField] private TextMeshProUGUI questTypeText, questNameText, questObjectiveText, questStatusText;
    [SerializeField] private Color mainQuestColor, sideQuestColor;
    private void Start()
    {
        if (questData != null)
        {
            SetQuestStatus(questStatus);
            SetQuestData(questData);
        }
    }
    public void SetQuestStatus(QuestStatus status)
    {
        questStatus = status;
    }
    public void SetQuestData(QuestsAndDialoguesSO questData)
    {
        this.questData = questData;
        questNameText.text = questData.questName;
        if (questStatus == QuestStatus.InProgress)
        {
            questTypeText.text = questData.questName;
            questStatusText.text = "Ongoing";
            questStatusText.color = Color.blue;
            questObjectiveText.text = questData.questObjective;
        }
        else if (questStatus == QuestStatus.Completed)
        {
            questStatusText.text = "Completed";
            questStatusText.color = Color.green;
            questObjectiveText.text = questData.questObjectiveCompleted;
        }
        else if (questStatus == QuestStatus.Failed)
        {
            questStatusText.text = "Failed";
            questStatusText.color = Color.red;
            questObjectiveText.text = questData.questObjectiveFailed;
        }
        else if (questStatus == QuestStatus.CompletedAndClaimed)
        {
            questStatusText.text = "Claimed";
            questStatusText.color = Color.yellow;
            questObjectiveText.text = questData.questObjectiveCompleted;
        }

        if (questData.isMainQuest)
        {
            questTypeText.text = "Main Quest";
            questTypeText.color = mainQuestColor;
        }
        else
        {
            questTypeText.text = "Side Quest";
            questTypeText.color = sideQuestColor;
        }
    }
}
