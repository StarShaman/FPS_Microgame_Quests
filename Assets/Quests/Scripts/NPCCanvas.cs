using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCCanvas : MonoBehaviour
{
    private Image mainQuestImage;
    private Image sideQuestImage;

    private void Start()
    {
        mainQuestImage = transform.GetChild(0).GetComponent<Image>();
        sideQuestImage = transform.GetChild(1).GetComponent<Image>();
        mainQuestImage.enabled = false;
        sideQuestImage.enabled = false;
    }

    public void MainQuestImageToggle(bool toggleAvailable)
    {
        if (toggleAvailable)
        {
            mainQuestImage.enabled = true;
        }
        else
        {
            mainQuestImage.enabled = false;
        }
    }
    public void SideQuestImageToggle(bool toggleAvailable)
    {
        if (toggleAvailable)
        {
            sideQuestImage.enabled = true;
        }
        else
        {
            sideQuestImage.enabled = false;
        }
    }
}
