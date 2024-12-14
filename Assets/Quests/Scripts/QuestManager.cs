using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public List<QuestInstance> activeQuests = new List<QuestInstance>();

    public void StartQuest(QuestsAndDialoguesSO quest)
    {
        QuestInstance instance = new QuestInstance { questData = quest, status = QuestStatus.InProgress };
        activeQuests.Add(instance);
    }

    public void CompleteQuest(QuestsAndDialoguesSO quest)
    {
        var instance = activeQuests.Find(q => q.questData == quest);
        if (instance != null)
            instance.status = QuestStatus.Completed;
    }
}
