using System.Collections.Generic;
using UnityEngine;
using static DialogueManager;

[CreateAssetMenu(fileName = "DialogueData")]
public class DialogueData : ScriptableObject
{
    public GenerationData.Generation generation;
    public BaseNPC.NPCType npcType;
    public int dayNumber;
    public List<Dialogue> dialogues;
}