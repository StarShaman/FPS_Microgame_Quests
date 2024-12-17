using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestLogUI : MonoBehaviour
{
    [SerializeField] private GameObject questLogUIContainer;
    [SerializeField] private GameObject questLogUIInfoContainer;
    private TextMeshProUGUI questLogSortInfoText;
    private TextMeshProUGUI questLogVisibilityInfoText;

    private RectTransform questLogUIContainerRectTransform;
    private Vector2 initialQuestLogUIContainerSizeDelta, hiddenQuestLogUIContainerSizeDelta;
    private Vector3 initialQuestLogUIContainerAnchoredPosition, hiddenQuestLogUIContainerAnchoredPosition;

    private RectTransform questLogUIInfoContainerRectTransform;
    private Vector3 initialQuestLogUIInfoContainerAnchoredPosition;
    private Vector3 hiddenQuestLogUIInfoContainerAnchoredPosition;
    private QuestLogItems questLogItems;
    private bool isQuestLogVisibile = true;
    private void Start()
    {
        questLogItems = GetComponent<QuestLogItems>();
        questLogUIContainerRectTransform = questLogUIContainer.GetComponent<RectTransform>();
        questLogUIInfoContainerRectTransform = questLogUIInfoContainer.GetComponent<RectTransform>();
        initialQuestLogUIInfoContainerAnchoredPosition = questLogUIInfoContainerRectTransform.anchoredPosition;
        hiddenQuestLogUIInfoContainerAnchoredPosition = new Vector3(0, 30, 0);
        initialQuestLogUIContainerAnchoredPosition = questLogUIContainerRectTransform.anchoredPosition;
        initialQuestLogUIContainerSizeDelta = questLogUIContainerRectTransform.sizeDelta;
        hiddenQuestLogUIContainerSizeDelta = new Vector2(100, 25);
        hiddenQuestLogUIContainerAnchoredPosition = new Vector3(0, 55.30169f, 0);
        questLogSortInfoText = questLogUIInfoContainer.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        questLogVisibilityInfoText = questLogUIInfoContainer.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            // when height becomes 25 then the pos Y should be 55.30169, found out about this by using the Rect tool in the Scene view
            if (questLogUIContainerRectTransform.sizeDelta == hiddenQuestLogUIContainerSizeDelta)
            {
                questLogItems.AddBackQuestLogItems();
                questLogUIContainerRectTransform.sizeDelta = initialQuestLogUIContainerSizeDelta;
                questLogUIContainerRectTransform.anchoredPosition = initialQuestLogUIContainerAnchoredPosition;
                // move sort info and hide info down with the Quest Log UI
                questLogUIInfoContainer.GetComponent<RectTransform>().anchoredPosition = initialQuestLogUIInfoContainerAnchoredPosition;
                questLogVisibilityInfoText.text = "Show (H)";
                isQuestLogVisibile = true;
            }
            else // Hide
            {
                questLogItems.RemoveAllQuestLogItems();
                questLogUIContainerRectTransform.sizeDelta = hiddenQuestLogUIContainerSizeDelta;
                questLogUIContainerRectTransform.anchoredPosition = hiddenQuestLogUIContainerAnchoredPosition;
                // move sort info and hide info up with the Quest Log UI
                questLogUIInfoContainer.GetComponent<RectTransform>().anchoredPosition = hiddenQuestLogUIInfoContainerAnchoredPosition;
                questLogVisibilityInfoText.text = "Hide (H)";
                isQuestLogVisibile = false;
            }
        }
    }

    public bool IsQuestLogVisible()
    {
        return isQuestLogVisibile;
    }
}
