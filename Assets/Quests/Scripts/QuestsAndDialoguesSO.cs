using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "QuestsAndDialoguesSO", menuName = "Custom/QuestsAndDialoguesSO", order = 1)]
public class QuestsAndDialoguesSO : ScriptableObject
{
    [Header("Quest Details")]
    public string questName;
    public string questDescription;

    [Header("Character Dialogue")]
    public string[] dialogueLines;
    public string[] questDialogueLines;
    public string questAccepted;
    public string questDeclined;
    public string questOngoing;
    public string questCompleted;
    public string questItemAlreadyObtainedLine;

    [Header("Rewards")]
    public string rewardItem;
}