using System.Collections.Generic;
using UnityEngine;
using static DialogueManager;

[CreateAssetMenu(fileName = "NPCData")]
public class NPCData : ScriptableObject
{
    public List<Dialogue> dialogues;
}