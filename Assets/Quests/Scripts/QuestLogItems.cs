using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestLogItems : MonoBehaviour
{
    private QuestLogUI questLogUI;
    [SerializeField] private GameObject questLogItemContainer;
    [SerializeField] private GameObject questLogItemPrefab;
    private void Start()
    {
        questLogUI = GetComponent<QuestLogUI>();
        QuestManager.Instance.OnQuestStatusChanged += QuestManager_OnQuestStatusChanged;
        QuestManager.Instance.OnQuestStarted += QuestManager_OnQuestStarted;
        QuestManager.Instance.OnQuestCompletedAndClaimed += QuestManager_OnQuestCompletedAndClaimed;
    }

    private void QuestManager_OnQuestCompletedAndClaimed(QuestsAndDialoguesSO questData)
    {
        if (questLogUI.IsQuestLogVisible())
        {
            RemoveQuestLogItem(questData);
        }
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
        foreach (Transform child in questLogItemContainer.transform)
        {
            QuestLogItemUI questLogItemUI = child.GetComponent<QuestLogItemUI>();
            if (questLogItemUI.questData == questData)
            {
                questLogItemUI.SetQuestData(questData);
                questLogItemUI.SetQuestStatus(status);
                return;
            }
        }
    }

    private void CreateQuestLogItem(QuestsAndDialoguesSO questData, QuestStatus questStatus)
    {
        GameObject questLogItem = Instantiate(questLogItemPrefab, questLogItemContainer.transform);
        QuestLogItemUI questLogItemUI = questLogItem.GetComponent<QuestLogItemUI>();
        questLogItemUI.SetQuestData(questData);
        questLogItemUI.SetQuestStatus(questStatus);
    }

    private void RemoveQuestLogItem(QuestsAndDialoguesSO questData)
    {
        foreach (Transform child in questLogItemContainer.transform)
        {
            if (child.GetComponent<QuestLogItemUI>().questData == questData)
            {
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
