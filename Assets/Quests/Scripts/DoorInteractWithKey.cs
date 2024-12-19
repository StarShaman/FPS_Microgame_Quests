using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractWithKey : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemSO requiredItemToOpen;
    private bool isDestroyed = false;

    public void Interact(Transform interactorTransform)
    {
        // Check if the player has the required key
        InventorySystem playerInventory = interactorTransform.GetComponent<InventorySystem>();
        if (playerInventory != null && playerInventory.HasItem(requiredItemToOpen) && !isDestroyed)
        {
            DestroyDoor();
        }
        else if (isDestroyed)
        {
            Debug.Log("The door has already been destroyed.");
        }
        else
        {
            Debug.Log("You need the key to destroy this door.");
        }
    }
    public string GetInteractText()
    {
        return isDestroyed ? "The door is destroyed" : "Destroy Door"; // Text based on door state
    }

    public Transform GetTransform()
    {
        return transform;
    }

    // Destroys the door and marks it as destroyed
    private void DestroyDoor()
    {
        isDestroyed = true;
        Destroy(gameObject); // Destroys the door object from the scene
    }
}