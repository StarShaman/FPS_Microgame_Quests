using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public List<QuestInstance> activeQuests = new List<QuestInstance>();
    private Dictionary<string, QuestStatus> questStatusMap = new Dictionary<string, QuestStatus>();
    public void StartQuest(QuestsAndDialoguesSO quest)
    {
        QuestInstance instance = new QuestInstance { questData = quest, status = QuestStatus.InProgress };
        activeQuests.Add(instance);

        // Add to the runtime map
        questStatusMap[quest.questId] = QuestStatus.InProgress;
    }

    public void CompleteQuest(QuestsAndDialoguesSO quest)
    {
        var instance = activeQuests.Find(q => q.questData == quest);
        if (instance != null)
        {
            instance.status = QuestStatus.Completed;
            questStatusMap[quest.questId] = QuestStatus.Completed;
        }
    }

    public QuestStatus GetQuestStatusByCode(string questID)
    {
        if (questStatusMap.TryGetValue(questID, out var status))
        {
            return status;
        }
        return QuestStatus.NotStarted; // Default for untracked quests
    }
}
