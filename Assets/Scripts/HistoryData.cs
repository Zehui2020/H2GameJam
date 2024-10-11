using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "History Data")]
public class HistoryData : ScriptableObject
{
    public GenerationData.Generation gen;
    public int day;
    public string currentYear;
    [TextArea(15,20)]
    public string desc;
}
