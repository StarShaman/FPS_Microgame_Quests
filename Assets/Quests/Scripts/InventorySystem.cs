using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance { get; private set; }

    private List<ItemSO> items = new List<ItemSO>();
    private InventoryUI inventoryUI;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        inventoryUI = gameObject.GetComponent<InventoryUI>();
    }

    public void AddItem(ItemSO item)
    {
        items.Add(item);
        UpdateVisuals();
    }

    public void RemoveItem(ItemSO item)
    {
        items.Remove(item);
        UpdateVisuals();
    }

    public bool HasItem(ItemSO item)
    {
        return items.Contains(item);
    }
    private void UpdateVisuals()
    {
        inventoryUI.UpdateInventoryDisplay(items);
    }
    public bool IsFull()
    {
        return items.Count >= 3;
    }
}
