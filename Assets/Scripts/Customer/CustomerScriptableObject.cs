using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[CreateAssetMenu(fileName = "Customer Type")]
public class CustomerScriptableObject : ScriptableObject
{
    public enum CustomerType
    {
        Positive,
        Negative,
        Impatient,
        TotalTypes
    }

    public CustomerType customerType;
    [Range(40, 100)]
    public float currentPatienceLevel;
    [Max(2)]
    public int wrongOrderLeeway;
}
