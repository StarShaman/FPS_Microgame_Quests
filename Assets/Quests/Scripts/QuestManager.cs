using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    // add event system to change quest log items
    public event Action<QuestsAndDialoguesSO,QuestStatus> OnQuestStatusChanged;
    public event Action<QuestsAndDialoguesSO> OnQuestStarted;
    public event Action<QuestsAndDialoguesSO> OnQuestCompletedAndClaimed;
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
        if (questStatusMap.ContainsKey(quest.questId))
        {
            Debug.LogWarning("Quest already started");
            return;
        }
        QuestInstance instance = new QuestInstance { questData = quest, status = QuestStatus.InProgress };

        QuestLogic questLogic = GetQuestLogicForType(quest);
        activeQuests.Add(instance);
        questStatusMap[quest.questId] = QuestStatus.InProgress;
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
