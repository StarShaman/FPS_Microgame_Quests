using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetrieveItemsQuestLogic : QuestLogic
{
    private ItemSO targetItem;
    private QuestInstance questInstance;
    private bool isItemRetrieved = false;
    public RetrieveItemsQuestLogic(ItemSO targetItem)
    {
        this.targetItem = targetItem;
    }

    public override void Initialize(QuestInstance questInstance)
    {
        this.questInstance = questInstance;
    }

    public override void UpdateLogic()
    {
        if (InventorySystem.Instance.HasItem(targetItem) && !isItemRetrieved)
        {
            isItemRetrieved = true;
            QuestManager.Instance.CompleteQuest(questInstance.questData);
        }
    }

    public override bool IsQuestCompleted()
    {
        return InventorySystem.Instance.HasItem(targetItem);
    }
}
