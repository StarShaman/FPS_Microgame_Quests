using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    private Image[] itemBoxes;
    private void Start()
    {
        itemBoxes = GetComponentsInChildren<Image>();
    }
    public void UpdateInventoryDisplay(List<ItemSO> items)
    {
        for (int i = 0; i < itemBoxes.Length; i++)
        {
            if (i < items.Count)
            {
                itemBoxes[i].sprite = items[i].sprite;
                itemBoxes[i].color = Color.white; // Set visible
            }
            else
            {
                itemBoxes[i].sprite = null;
                itemBoxes[i].color = Color.clear; // Hide empty boxes
            }
        }
    }
}
