using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialoguePopup : MonoBehaviour
{
    [System.Serializable]
    public struct DialoguePopupData
    {
        public string dialogue;
        public string speakerName;
        public Sprite speakerSprite;
        public Transform questDestination;
        public bool goToNext;
        public float waitDuration;

        public UnityEvent OnFinish;
    }

    [SerializeField] private Canvas popupCanvas;
    [SerializeField] private Animator animator;
    [SerializeField] private Image characterIcon;
    public TypewriterEffect dialogueText;
    [SerializeField] private QuestPointer questPointer;

    private Coroutine HideCoroutine;

    public void ShowDialoguePopup(DialoguePopupData dialogue)
    {
        if (HideCoroutine != null)
        {
            StopCoroutine(HideCoroutine);
            animator.ResetTrigger("hide");
            SetCanvasEnabled(true);
            HideCoroutine = null;
        }

        SetCanvasEnabled(true);
        animator.SetTrigger("show");
        characterIcon.sprite = dialogue.speakerSprite;
        dialogueText.ShowMessage(dialogue.speakerName, dialogue.dialogue);

        if (questPointer != null)
        {
            if (dialogue.questDestination != null)
                questPointer.Show(dialogue.questDestination);
            else
                questPointer.Hide();
        }
    }

    public void HidePopup()
    {
        HideCoroutine = StartCoroutine(HideRoutine());
    }

    public void HidePopupImmediately()
    {
        if (!popupCanvas.enabled)
            return;

        if (HideCoroutine != null)
        {
            StopCoroutine(HideCoroutine);
            HideCoroutine = null;
        }

        animator.ResetTrigger("hide");
        SetCanvasEnabled(false);
    }

    public void SetCanvasEnabled(bool enable)
    {
        popupCanvas.enabled = enable;
    }

    private IEnumerator HideRoutine()
    {
        yield return new WaitForSeconds(2f);
        animator.SetTrigger("hide");
        yield return new WaitForSeconds(0.3f);
        SetCanvasEnabled(false);
        HideCoroutine = null;
    }
}