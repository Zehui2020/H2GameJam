using DesignPatterns.ObjectPool;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static DialogueManager;

public class DialogueManager : MonoBehaviour
{
    [System.Serializable]
    public struct DialogueEvent
    {
        public bool playOnce;
        private bool isInvoked;
        public UnityEvent onDialogueDone;

        public void InvokeEvent()
        {
            if (isInvoked && playOnce)
                return;

            onDialogueDone?.Invoke();
            isInvoked = true;
        }

        public void ResetEvent()
        {
            isInvoked = false;
        }
    }

    [System.Serializable]
    public struct Dialogue
    {
        [System.Serializable]
        public struct DialogueChoice
        {
            public string choice;
            public int nextDialogueIndex;
        }

        public bool showOnce;
        public bool isLoopingDialogue;
        [HideInInspector] public bool isShown;

        public string speakerName;
        public Sprite speakerIcon;
        public bool breakAfterDialogue;
        public DialogueEvent onDialogueDone;
        [TextArea(1, 10)] public string dialogue;
        public List<DialogueChoice> playerChoices;

        public void SetIsShown(bool shown)
        {
            isShown = shown;
        }

        public void ResetDialogue()
        {
            isShown = false;
            onDialogueDone.ResetEvent();
        }
    }

    [SerializeField] private TypewriterEffect npcDialogue;

    [SerializeField] private Image playerPortrait;
    [SerializeField] private Image npcPortrait;
    [SerializeField] private Color showColor;
    [SerializeField] private Color hideColor;

    [SerializeField] private Canvas dialogueCanvas;
    [SerializeField] private RectTransform dialogueChoiceParent;
    [SerializeField] private GameObject dialogueBox;

    private List<DialogueChoice> dalogueChoices = new();
    [SerializeField] private BaseNPC currentNPC;

    private Dialogue currentDialogue;
    private bool canShowNextDialogue;

    private void Start()
    {
        npcDialogue.OnFinishTyping.AddListener(CheckShowChoices);
    }

    public void SetTalkingNPC(BaseNPC npc)
    {
        currentNPC = npc;
        dialogueCanvas.enabled = true;
        ShowDialogue(npc.GetCurrentDialogue());
    }

    public void CheckShowChoices()
    {
        if (currentDialogue.playerChoices.Count == 0)
        {
            canShowNextDialogue = true;
            return;
        }

        // Show player choices
        ShowDialogueChoices(currentDialogue);
        playerPortrait.color = showColor;
        npcPortrait.color = hideColor;
        canShowNextDialogue = false;
    }

    public void ShowNextDialogue()
    {
        if (!canShowNextDialogue)
            return;

        if (currentDialogue.showOnce && currentDialogue.isShown)
            return;

        currentDialogue.onDialogueDone.InvokeEvent();

        if (currentDialogue.breakAfterDialogue)
        {
            if (!currentDialogue.isLoopingDialogue)
                currentNPC.IncrementIndex(1);

            currentNPC.OnEndDialogue();
            return;
        }

        ShowDialogue(currentNPC.GetNextDialogue());
    }

    public void ShowDialogue(Dialogue dialogue)
    {
        currentDialogue = dialogue;

        canShowNextDialogue = false;
        npcDialogue.SetSpeakerName(dialogue.speakerName);

        if (dialogue.dialogue == string.Empty)
        {
            dialogueBox.SetActive(false);
            return;
        }

        dialogueBox.SetActive(true);
        npcDialogue.ShowMessage(dialogue.speakerName, dialogue.dialogue);
        playerPortrait.color = hideColor;
        npcPortrait.color = showColor;
    }

    public void ShowDialogueChoices(Dialogue dialogue)
    {
        // Show player choices
        foreach (Dialogue.DialogueChoice choice in dialogue.playerChoices)
        {
            DialogueChoice dialogueChoice = ObjectPool.Instance.GetPooledObject("DialogueChoice", false) as DialogueChoice;
            dalogueChoices.Add(dialogueChoice);
            dialogueChoice.InitChoice(choice);
            dialogueChoice.transform.SetParent(dialogueChoiceParent);
            dialogueChoice.gameObject.SetActive(true);

            dialogueChoice.OnSelectEvent += (currentChoice) => 
            {
                canShowNextDialogue = true;
                ShowDialogue(currentNPC.GetDialogueFromIndex(currentChoice.nextDialogueIndex)); 
                foreach (DialogueChoice dialogue in dalogueChoices)
                    dialogue.ReturnToPool();
            };
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(dialogueChoiceParent);
    }

    private void Update()
    {
        if (npcDialogue.IsTyping())
            return;

        if (Input.GetMouseButtonDown(0) || Input.touches.Count() > 0)
            ShowNextDialogue();
    }

    public void HideDialogue()
    {
        dialogueCanvas.enabled = false;
    }
}