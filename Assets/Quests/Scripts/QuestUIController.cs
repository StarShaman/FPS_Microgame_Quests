using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Turns on and off quest UI elements regarding which quest is available
public class QuestUIController : MonoBehaviour
{
    [SerializeField] private NPCInteractable[] npcs;
    private void Start()
    {
        foreach (NPCInteractable npc in npcs)
        {
            NPCCanvas npcCanvas = npc.gameObject.GetComponentInChildren<NPCCanvas>();
            if (npc.GetQuestData().isMainQuest)
            {
                if (npc.GetQuestData().questOrderIfMainQuest == 1) // 1st main quest UI is turned on
                {
                    npcCanvas.MainQuestImageToggle(true);
                }
            }
            else
            {
                npcCanvas.SideQuestImageToggle(true);
            }
        }
        QuestManager.Instance.OnQuestStarted += Instance_OnQuestStarted;
        QuestManager.Instance.OnQuestStatusChanged += QuestManager_OnQuestStatusChanged;
    }

    private void Instance_OnQuestStarted(QuestsAndDialoguesSO questData)
    {
        foreach (NPCInteractable npc in npcs)
        {
            QuestsAndDialoguesSO npcQuestData = npc.GetQuestData();
            NPCCanvas npcCanvas = npc.gameObject.GetComponentInChildren<NPCCanvas>();
            if (npcQuestData.questId == questData.questId)
            {
                if (npcQuestData.isMainQuest)
                {
                    npcCanvas.MainQuestImageToggle(false);
                }
                else
                {
                    npcCanvas.SideQuestImageToggle(false);
                }
            }
        }
    }

    private void QuestManager_OnQuestStatusChanged(QuestsAndDialoguesSO questData, QuestStatus status)
    {
        if (status == QuestStatus.CompletedAndClaimed)
        {
            TurnOnNextQuestUI(questData);
        }
    }

    private void TurnOnNextQuestUI(QuestsAndDialoguesSO questData)
    {
        foreach (NPCInteractable npc in npcs)
        {
            QuestsAndDialoguesSO npcQuestData = npc.GetQuestData();
            NPCCanvas npcCanvas = npc.gameObject.GetComponentInChildren<NPCCanvas>();

            // turning on the next main quest UI
            if (npcQuestData.questOrderIfMainQuest == questData.questOrderIfMainQuest + 1 && questData.isMainQuest)
            {
                npcCanvas.MainQuestImageToggle(true);
            }
        }
    }
}
