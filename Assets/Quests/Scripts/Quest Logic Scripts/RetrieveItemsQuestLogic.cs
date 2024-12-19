using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetrieveItemsQuestLogic : QuestLogic
{
    private Dictionary<ItemSO, int> targetItems;
    private QuestInstance questInstance;
    private bool IsLogicOnlyAppliedOnce = false;
    public RetrieveItemsQuestLogic(ItemSO[] targetItems, int[] howMuchEachOne)
    {
        for (int i = 0; i < targetItems.Length; i++)
        {
            this.targetItems.Add(targetItems[i], howMuchEachOne[i]);
        }
    }

    public override void Initialize(QuestInstance questInstance)
    {
        this.questInstance = questInstance;
    }

    public override void UpdateLogic()
    {
        if (!IsLogicOnlyAppliedOnce && AreAllItemsRetrieved())
        {
            IsLogicOnlyAppliedOnce = true;
            QuestManager.Instance.CompleteQuest(questInstance.questData);
            foreach(var item in targetItems)
            {
                InventorySystem.Instance.RemoveItem(item.Key);
            }
        }
    }

    private bool AreAllItemsRetrieved()
    {
        foreach (var item in targetItems)
        {
            if (!InventorySystem.Instance.HasItem(item.Key))
            {
                return false;
            }
        }
        return false;
    }

    public override bool IsQuestCompleted()
    {
        return AreAllItemsRetrieved();
    }
}
