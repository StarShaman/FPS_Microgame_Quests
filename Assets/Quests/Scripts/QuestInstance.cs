using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestInstance : MonoBehaviour
{
    public QuestsAndDialoguesSO questData;
    public QuestStatus status;
}

public enum QuestStatus { NotStarted, InProgress, Completed, Failed, CompletedAndClaimed }
