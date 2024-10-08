using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//Description: Contains all dialogues: Types and text

public class CustomerDialogueController : MonoBehaviour
{
    public enum DialogueTypes
    {
        PosPriceRemarks,
        NegPriceRemarks,
        VenueRemarks,
        ImpatientRemarks,
        PosReviewRemarks,
        NegReviewRemarks,
        WrongItemRemarks,
        NormalGreetingRemarks,
        TotalTypes
    }

    [System.Serializable]
    public struct CustomerDialogue
    {
        public DialogueTypes type;
        public string text;


        public bool bindToGeneration;
        public bool bindToGen1;
        public bool bindToGen2;
        public bool bindToGen3;
    }

    public static CustomerDialogueController Instance { get; private set; }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    //Store all customer dialogue
    [SerializeField] private List<CustomerDialogue> customerDialogues;
}
