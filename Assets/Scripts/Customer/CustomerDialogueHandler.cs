using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerDialogueHandler : MonoBehaviour
{
    [SerializeField] private Text dialogueText;

    private string sentence;

    public void InitNewDialogue(CustomerDialogueController.DialogueType _type)
    {
        sentence = CustomerDialogueController.Instance.GetDialogue(_type);
    }
}
