using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    public List<QuestInstance> activeQuests = new List<QuestInstance>();
    private Dictionary<string, QuestStatus> questStatusMap = new Dictionary<string, QuestStatus>();

    [SerializeField] private QuestStatusPopUpUI questStatusPopUpUI;

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
    public void StartQuest(QuestsAndDialoguesSO quest)
    {
        if (questStatusMap.ContainsKey(quest.questId))
        {
            Debug.LogWarning("Quest already started");
            return;
        }
        QuestInstance instance = new QuestInstance { questData = quest, status = QuestStatus.InProgress };
        activeQuests.Add(instance);

        // Add to the runtime map
        questStatusMap[quest.questId] = QuestStatus.InProgress;

        questStatusPopUpUI.ShowQuestPopUpWithFadeIn(quest, 0);
    }

    public void CompleteQuest(QuestsAndDialoguesSO quest)
    {
        var instance = activeQuests.Find(q => q.questData == quest);
        if (instance != null)
        {
            instance.status = QuestStatus.Completed;
            questStatusMap[quest.questId] = QuestStatus.Completed;
            questStatusPopUpUI.ShowQuestPopUpWithFadeIn(quest, 1);
        }
    }
    public void FailQuest(QuestsAndDialoguesSO quest)
    {
        var instance = activeQuests.Find(q => q.questData == quest);
        if (instance != null)
        {
            instance.status = QuestStatus.Failed;
            questStatusMap[quest.questId] = QuestStatus.Failed;
            questStatusPopUpUI.ShowQuestPopUpWithFadeIn(quest, 2);
        }
    }
    public void ClaimQuest(QuestsAndDialoguesSO quest)
    {
        var instance = activeQuests.Find(q => q.questData == quest);
        if (instance != null)
        {
            instance.status = QuestStatus.CompletedAndClaimed;
            questStatusMap[quest.questId] = QuestStatus.CompletedAndClaimed;
            questStatusPopUpUI.ShowQuestPopUpWithFadeIn(quest, 3);
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
