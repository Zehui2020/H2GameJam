using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//Description: Contains all dialogues: Types and text

public class CustomerDialogueController : MonoBehaviour
{
    public enum DialogueType
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
        public DialogueType type;
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


    public string GetDialogue(DialogueType _type)
    {
        //get current generation
        int gen = (int)PlayerStats.playerStatsInstance.currentGeneration;

        List<string> typeDialogue = new List<string>();
        foreach (CustomerDialogue d in customerDialogues)
        {
            //check if same dialogue type
            if (d.type == _type)
            {
                if (!d.bindToGeneration ||
                    (d.bindToGen1 && gen == 0) ||
                    (d.bindToGen2 && gen == 1) ||
                    (d.bindToGen3 && gen == 2))
                {
                    typeDialogue.Add(d.text);
                }
            }
        }

        return typeDialogue[Random.Range(0, typeDialogue.Count)];
    }
}
