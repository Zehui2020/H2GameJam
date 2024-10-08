using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static DialogueManager;

public class BaseNPC : MonoBehaviour
{
    [SerializeField] private Animator dialogueUIAnimator;
    [SerializeField] private Animator interactButtonAnimator;
    [SerializeField] private DialogueManager dialogueManager;

    public UnityEvent InteractEvent;

    [SerializeField] private int dialogueIndex;
    [SerializeField] private List<Dialogue> dialogues;

    public void OnEnterRange()
    {
        interactButtonAnimator.SetBool("inRange", true);
        LayoutRebuilder.ForceRebuildLayoutImmediate(interactButtonAnimator.GetComponent<RectTransform>());
    }

    public void OnInteract()
    {
        InteractEvent?.Invoke();
        dialogueUIAnimator.SetBool("isTalking", true);
        dialogueManager.SetTalkingNPC(this);
    }

    public void OnLeaveRange()
    {
        interactButtonAnimator.SetBool("inRange", false);
    }

    public void OnEndDialogue()
    {
        dialogueUIAnimator.SetBool("isTalking", false);
    }

    public Dialogue GetCurrentDialogue()
    {
        return dialogues[dialogueIndex];
    }

    public void IncrementIndex(int amount)
    {
        dialogueIndex += amount;
    }

    public Dialogue GetNextDialogue()
    {
        if (dialogueIndex + 1 > dialogues.Count - 1)
        {
            OnEndDialogue();
        }
        else
            IncrementIndex(1);

        Dialogue dialogue = dialogues[dialogueIndex];
        dialogue.SetIsShown(true);
        dialogues[dialogueIndex] = dialogue;

        return dialogues[dialogueIndex];
    }

    public Dialogue GetDialogueFromIndex(int index)
    {
        dialogueIndex = index;

        Dialogue dialogue = dialogues[dialogueIndex];
        dialogue.SetIsShown(true);
        dialogues[dialogueIndex] = dialogue;

        return dialogues[dialogueIndex];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        OnEnterRange();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        OnLeaveRange();
    }
}