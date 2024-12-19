using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "QuestsAndDialoguesSO", menuName = "Quests/QuestsAndDialoguesSO", order = 1)]
public class QuestsAndDialoguesSO : ScriptableObject
{
    [Header("Quest Details")]
    public string questName;
    public string questId;
    public QuestType questType;
    public string questDescription;
    public bool isMainQuest;
    public int questOrderIfMainQuest;
    public bool isRepeatableQuest;

    [Header("Extra Variables")]
    public float timeLimit; // for timed quests, could be renamed to a generic name if you want to add more types of quests
    public int targetKillCount; // for kill quests
    public ItemSO targetItem; // for item retrieval quests

    [Header("Character Dialogue")]
    public string[] dialogueLines;
    public string[] questDialogueLines;
    public string questAccepted;
    public string questDeclined;
    public string questFailed;
    public string questOngoing;
    public string questCompleted;
    public string questItemAlreadyObtainedLine;

    [Header("Objective (Status) Description")]
    public string questObjective;
    public string questObjectiveCompleted;
    public string questObjectiveFailed;
    public string questObjectiveRewardClaimedAndCompleted;

    [Header("Rewards")] // can be expanded to include more types of rewards, like experience points, etc.
    public ItemSO rewardItemSO;
    public GameObject rewardItemPrefab;
}
