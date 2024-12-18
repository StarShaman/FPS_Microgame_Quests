using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    // add event system to change quest log items
    public event Action<QuestsAndDialoguesSO,QuestStatus> OnQuestStatusChanged;
    public event Action<QuestsAndDialoguesSO> OnQuestStarted;
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
    private void InvokeStatusChangeToQuest(QuestsAndDialoguesSO quest, QuestStatus status)
    {
        OnQuestStatusChanged?.Invoke(quest, status);
    }
    public void StartQuest(QuestsAndDialoguesSO quest)
    {
        if (quest.isMainQuest)
        {
            // Find the highest `questOrderIfMainQuest` from completed and claimed main quests
            int highestCompletedOrder = 0;

            foreach (var questInstance in activeQuests)
            {
                if (questInstance.questData.isMainQuest &&
                    questInstance.status == QuestStatus.CompletedAndClaimed)
                {
                    highestCompletedOrder = Mathf.Max(highestCompletedOrder, questInstance.questData.questOrderIfMainQuest);
                }
            }
            // Check if the current quest can start
            if (quest.questOrderIfMainQuest > highestCompletedOrder + 1)
            {
                Debug.LogWarning($"Cannot start quest {quest.questName}. Previous main quest(s) have not been completed and claimed.");
                return;
            }
        }
        if (questStatusMap.ContainsKey(quest.questId))
        {
            Debug.LogWarning($"Quest {quest.questName} already started.");
            return;
        }

        GameObject questGameObject = new GameObject($"QuestInstance_{quest.questName}");
        QuestInstance instance = questGameObject.AddComponent<QuestInstance>();

        // Initializing quest instance
        instance.questData = quest;
        instance.status = QuestStatus.InProgress;

        // Assigning and initializing the quest logic
        QuestLogic questLogic = GetQuestLogicForType(quest);
        instance.Initialize(questLogic);

        // Tracking active quest and update quest status
        activeQuests.Add(instance);
        questStatusMap[quest.questId] = QuestStatus.InProgress;

        // Trigger UI and events
        questStatusPopUpUI.ShowQuestPopUpWithFadeIn(quest, 0);
        OnQuestStarted?.Invoke(quest);
    }

    private QuestLogic GetQuestLogicForType(QuestsAndDialoguesSO quest)
    {
        switch (quest.questType)
        {
            case QuestType.TimedKillEnemies:
                //return new TimedKillEnemiesQuestLogic();
            case QuestType.RetrieveItems:
                return new RetrieveItemsQuestLogic(quest.targetItem);
            case QuestType.Escort:
                //return new EscortQuestLogic();
            case QuestType.TalkToAnotherNPC:
                //return new TalkToAnotherNPCQuestLogic();
            default:
                Debug.LogError("Quest type not implemented");
                return null;
        }
    }

    public void CompleteQuest(QuestsAndDialoguesSO quest)
    {
        var instance = activeQuests.Find(q => q.questData == quest);
        if (instance != null)
        {
            instance.status = QuestStatus.Completed;
            questStatusMap[quest.questId] = QuestStatus.Completed;
            questStatusPopUpUI.ShowQuestPopUpWithFadeIn(quest, 1);
            InvokeStatusChangeToQuest(quest, QuestStatus.Completed);
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
            InvokeStatusChangeToQuest(quest, QuestStatus.Failed);
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
            InvokeStatusChangeToQuest(quest, QuestStatus.CompletedAndClaimed);
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
