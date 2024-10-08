using UnityEngine;
using UnityEngine.Events;
using static DialogueManager;

public class BaseNPC : MonoBehaviour
{
    [SerializeField] private NPCData npcData;
    [SerializeField] private Animator dialogueUIAnimator;
    [SerializeField] private Animator interactButtonAnimator;

    public UnityEvent InteractEvent;
    public UnityEvent LeaveEvent;

    private int dialogueIndex;

    public void OnEnterRange()
    {
        interactButtonAnimator.SetBool("inRange", true);
    }

    public void OnInteract()
    {
        InteractEvent?.Invoke();
        dialogueUIAnimator.SetBool("isTalking", true);
    }

    public void OnLeaveRange()
    {
        interactButtonAnimator.SetBool("inRange", false);
    }

    public Dialogue GetCurrentDialogue()
    {
        return npcData.dialogues[dialogueIndex];
    }

    public void IncrementIndex(int amount)
    {
        dialogueIndex += amount;
    }

    public Dialogue GetNextDialogue()
    {
        if (dialogueIndex + 1 > npcData.dialogues.Count - 1)
        {
            LeaveEvent?.Invoke();
            dialogueUIAnimator.SetBool("isTalking", false);
        }
        else
            IncrementIndex(1);

        Dialogue dialogue = npcData.dialogues[dialogueIndex];
        dialogue.SetIsShown(true);
        npcData.dialogues[dialogueIndex] = dialogue;

        return npcData.dialogues[dialogueIndex];
    }

    public Dialogue GetDialogueFromIndex(int index)
    {
        dialogueIndex = index;

        Dialogue dialogue = npcData.dialogues[dialogueIndex];
        dialogue.SetIsShown(true);
        npcData.dialogues[dialogueIndex] = dialogue;

        return npcData.dialogues[dialogueIndex];
    }
}