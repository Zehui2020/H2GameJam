using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.Rendering;

public class CustomerDialogueHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TypewriterEffect npcDialogue;

    private Animator animator;


    private string sentence;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void InitNewDialogue(CustomerDialogueController.DialogueType _type)
    {
        sentence = CustomerDialogueController.Instance.GetDialogue(_type);

        dialogueText.text = string.Empty;

        npcDialogue.ShowMessage(string.Empty, sentence);
        //start animating in
        animator.SetTrigger("Show");
    }
}
