using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private DialoguePopup dialoguePopup;
    [SerializeField] private List<DialoguePopup.DialoguePopupData> dialoguePopups = new();
    [SerializeField] private float startDelay = 2f;
    private int currentDialogue;

    private IEnumerator Start()
    {
        currentDialogue = 0;
        dialoguePopup.dialogueText.OnFinishTyping.AddListener(OnFinishTyping);

        yield return new WaitForSecondsRealtime(startDelay);

        dialoguePopup.ShowDialoguePopup(dialoguePopups[currentDialogue]);
    }

    private void OnFinishTyping()
    {
        StartCoroutine(FinishTypingRoutine());
    }

    private IEnumerator FinishTypingRoutine()
    {
        dialoguePopups[currentDialogue].OnFinish?.Invoke();

        currentDialogue++;

        yield return new WaitForSecondsRealtime(dialoguePopups[currentDialogue - 1].waitDuration);

        if (dialoguePopups[currentDialogue - 1].goToNext)
            dialoguePopup.ShowDialoguePopup(dialoguePopups[currentDialogue]);
        else
            dialoguePopup.HidePopup();
    }
}