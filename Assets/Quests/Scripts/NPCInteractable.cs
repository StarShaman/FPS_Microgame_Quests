using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string interactText;
    [SerializeField] private QuestsAndDialoguesSO questData;
    private Animator animator;
    private NPCHeadLookAt npcHeadLookAt;
    private string npcName;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        npcHeadLookAt = GetComponent<NPCHeadLookAt>();
        npcName = gameObject.name;
    }
    
    public void Interact(Transform interactorTransform)
    {
        DialogueManager.Instance.StartDialogue(npcName, questData);

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
