using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestLogic : MonoBehaviour
{
    public abstract void Initialize(QuestInstance questInstance);
    public abstract void UpdateLogic();
    public abstract bool IsQuestCompleted();
}
