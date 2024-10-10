using System;
using System.Collections;
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
        PlayerStats.playerStatsInstance.playerMarketState = PlayerStats.PlayerMarketState.InMenu;
        dialogueUIAnimator.SetBool("isTalking", true);
        dialogueManager.SetTalkingNPC(this);
    }

    public void OnInteract(int index)
    {
        StartCoroutine(DelayedInteract(index));
    }

    private IEnumerator DelayedInteract(int index)
    {
        yield return new WaitForSeconds(0.4f);

        GetDialogueFromIndex(index);
        InteractEvent?.Invoke();
        PlayerStats.playerStatsInstance.playerMarketState = PlayerStats.PlayerMarketState.InMenu;
        dialogueUIAnimator.SetBool("isTalking", true);
        dialogueManager.SetTalkingNPC(this);
    }

    public void GoToWork()
    {
        PlayerStats.playerStatsInstance.GoToCook();
    }

    public void GoToTutorialCooking()
    {
        SceneLoader.Instance.LoadScene("TutorialCookingScene");
    }

    public void OnLeaveRange()
    {
        interactButtonAnimator.SetBool("inRange", false);
        PlayerStats.playerStatsInstance.playerMarketState = PlayerStats.PlayerMarketState.Walk;
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