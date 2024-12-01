using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string interactText;
    private Animator animator;
    private NPCHeadLookAt npcHeadLookAt;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        npcHeadLookAt = GetComponent<NPCHeadLookAt>();
    }
    
    public void Interact(Transform interactorTransform)
    {
        // In the future, create a dialogue system here
        Debug.Log("Interacting with NPC");

        animator.SetTrigger("Talk");

        float playerHeight = 1.7f;
        npcHeadLookAt.LookAtPosition(interactorTransform.position + Vector3.up * playerHeight);
    }

    public string GetInteractText()
    {
        return interactText;
    }
    public Transform GetTransform()
    {
        return transform;
    }
}
