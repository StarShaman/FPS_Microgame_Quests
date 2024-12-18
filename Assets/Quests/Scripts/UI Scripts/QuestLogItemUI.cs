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

    public void UpdateQuestData(QuestsAndDialoguesSO questData, QuestStatus status)
    {
        questStatus = status;
        this.questData = questData;
        questNameText.text = questData.questName;
        Color statusColor = Color.white;
        string statusText = "";
        string objectiveText = "";

        switch (questStatus)
        {
            case QuestStatus.InProgress:
                statusText = "Ongoing";
                statusColor = Color.blue;
                objectiveText = questData.questObjective;
                break;
            case QuestStatus.Completed:
                statusText = "Completed";
                statusColor = Color.green;
                objectiveText = questData.questObjectiveCompleted;
                break;
            case QuestStatus.Failed: // left for future use, if we want to keep failed quests in the log; for CompletedAndClaimed too
                statusText = "Failed";
                statusColor = Color.red;
                objectiveText = questData.questObjectiveFailed;
                break;
            case QuestStatus.CompletedAndClaimed:
                statusText = "Claimed";
                statusColor = Color.yellow;
                objectiveText = questData.questObjectiveCompleted;
                break;
        }
        questTypeText.text = questData.questName;
        questStatusText.text = statusText;
        questStatusText.color = statusColor;
        questObjectiveText.text = objectiveText;

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
