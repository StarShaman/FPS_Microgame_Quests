using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestLogItems : MonoBehaviour
{
    private QuestLogUI questLogUI;
    [SerializeField] private GameObject questLogItemContainer;
    [SerializeField] private GameObject questLogItemPrefab;
    private List<QuestLogItemUI> questLogUIItems = new List<QuestLogItemUI>();
    private void Start()
    {
        questLogUI = GetComponent<QuestLogUI>();
        QuestManager.Instance.OnQuestStatusChanged += QuestManager_OnQuestStatusChanged;
        QuestManager.Instance.OnQuestStarted += QuestManager_OnQuestStarted;
    }

    private void QuestManager_OnQuestStarted(QuestsAndDialoguesSO questData)
    {
        if (questLogUI.IsQuestLogVisible())
        {
            CreateQuestLogItem(questData, QuestStatus.InProgress);
        }
    }

    private void QuestManager_OnQuestStatusChanged(QuestsAndDialoguesSO questData, QuestStatus status)
    {
        if (questLogUI.IsQuestLogVisible())
        {
            if (status == QuestStatus.CompletedAndClaimed || status == QuestStatus.Failed) // could be removed hypothetically if we want to keep completed quests in the log
            {
                RemoveQuestLogItem(questData);
            }
            else
            {
                // find the quest log item and update its status
                foreach (QuestLogItemUI questLogItem in questLogUIItems)
                {
                    Debug.Log(questLogItem.questData);
                    if (questLogItem.questData == questData)
                    {
                        Debug.Log(questLogItem);
                        questLogItem.UpdateQuestData(questData, status);
                        return;
                    }
                }
            }
        }
    }

    private void CreateQuestLogItem(QuestsAndDialoguesSO questData, QuestStatus questStatus)
    {
        GameObject questLogItem = Instantiate(questLogItemPrefab, questLogItemContainer.transform);
        QuestLogItemUI questLogItemUI = questLogItem.GetComponent<QuestLogItemUI>();
        questLogUIItems.Add(questLogItemUI);
        questLogItemUI.UpdateQuestData(questData, questStatus);
    }

    private void RemoveQuestLogItem(QuestsAndDialoguesSO questData)
    {
        foreach (Transform child in questLogItemContainer.transform)
        {
            if (child.GetComponent<QuestLogItemUI>().questData == questData)
            {
                questLogUIItems.Remove(child.GetComponent<QuestLogItemUI>());
                Destroy(child.gameObject);
                return;
            }
        }
    }

    public void RemoveAllQuestLogItems()
    {
        foreach (Transform child in questLogItemContainer.transform)
        {
            Destroy(child.gameObject);
        }
        questLogUIItems.Clear();
    }
    public void AddBackQuestLogItems()
    {
        for (int i = 0; i < QuestManager.Instance.activeQuests.Count; i++)
        {
            if (QuestManager.Instance.activeQuests[i].status == QuestStatus.CompletedAndClaimed 
                || QuestManager.Instance.activeQuests[i].status == QuestStatus.Failed)
                return;
            CreateQuestLogItem(QuestManager.Instance.activeQuests[i].questData, QuestManager.Instance.activeQuests[i].status);
        }

    }
}
