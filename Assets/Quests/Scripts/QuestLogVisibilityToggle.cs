using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestLogVisibilityToggle : MonoBehaviour
{
    private QuestLogUI questLogUI;
    private void Start()
    {
        questLogUI = GetComponent<QuestLogUI>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            questLogUI.ToggleVisibility();
        }
    }
}
