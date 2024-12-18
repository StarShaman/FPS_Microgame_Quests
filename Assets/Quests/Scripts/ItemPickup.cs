using System.Collections;
using System.Collections.Generic;
using Unity.FPS.Gameplay;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private ItemSO itemData;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerCharacterController playerCharacterController) == true)
        {
            if (InventorySystem.Instance.IsFull())
            {
                return; // Do not pick up item if inventory is full
            }

            InventorySystem.Instance.AddItem(itemData);
            Destroy(gameObject);
        }
    }
}
