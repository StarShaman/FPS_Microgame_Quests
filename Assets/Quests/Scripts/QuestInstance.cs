using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestInstance : MonoBehaviour
{
    public QuestsAndDialoguesSO questData;
    public QuestStatus status;

    private QuestLogic questLogic;

    public void Initialize(QuestLogic logic)
    {
        questLogic = logic;
        questLogic.Initialize(this);
    }

    private void Update()
    {
        questLogic?.UpdateLogic();
    }
}

public enum QuestStatus { NotStarted, InProgress, Completed, Failed, CompletedAndClaimed }
public enum QuestType { TimedKillEnemies, RetrieveItems, Escort, TalkToAnotherNPC }
