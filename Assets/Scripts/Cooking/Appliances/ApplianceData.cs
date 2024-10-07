using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ApplianceData")]
public class ApplianceData : ScriptableObject
{
    public enum ApplianceType
    {
        Pot,
        Grill,
        DeepFryer,
        Wok,
        Steamer,
        Pan
    }

    public List<Ingredient> allowedIngredients = new();
    public ApplianceType type;
    public float cookDuration;
    public float burnDuration;
}