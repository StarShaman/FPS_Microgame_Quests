using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestLogUI : MonoBehaviour
{
    [SerializeField] private GameObject questLogUIContainer;
    [SerializeField] private GameObject questLogUIInfoContainer;
    private TextMeshProUGUI questLogVisibilityInfoText;

    private RectTransform questLogUIContainerRectTransform, questLogUIInfoContainerRectTransform;
    private Vector2 initialQuestLogUIContainerSizeDelta, hiddenQuestLogUIContainerSizeDelta;
    private Vector3 initialQuestLogUIContainerAnchoredPosition, hiddenQuestLogUIContainerAnchoredPosition,
        initialQuestLogUIInfoContainerAnchoredPosition, hiddenQuestLogUIInfoContainerAnchoredPosition;
    private QuestLogItems questLogItems;
    private bool isQuestLogVisible = true;
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
        questLogVisibilityInfoText = questLogUIInfoContainer.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }
    public void ToggleVisibility()
    {
        isQuestLogVisible = !isQuestLogVisible;

        questLogUIContainerRectTransform.sizeDelta = isQuestLogVisible ? initialQuestLogUIContainerSizeDelta : hiddenQuestLogUIContainerSizeDelta;
        questLogUIContainerRectTransform.anchoredPosition = isQuestLogVisible ? initialQuestLogUIContainerAnchoredPosition : hiddenQuestLogUIContainerAnchoredPosition;
        questLogUIInfoContainerRectTransform.anchoredPosition = isQuestLogVisible ? initialQuestLogUIInfoContainerAnchoredPosition : hiddenQuestLogUIInfoContainerAnchoredPosition;
        questLogVisibilityInfoText.text = isQuestLogVisible ? "Hide (H)" : "Show (H)";

        if (isQuestLogVisible)
        {
            questLogItems.AddBackQuestLogItems();
        }
        else
        {
            questLogItems.RemoveAllQuestLogItems();
        }
    }

    public bool IsQuestLogVisible()
    {
        return isQuestLogVisible;
    }
}
