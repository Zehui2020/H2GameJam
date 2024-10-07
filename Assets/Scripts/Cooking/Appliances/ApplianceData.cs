using System.Collections.Generic;
using UnityEngine;

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

    public List<Dish> allowedDishes = new();
    public ApplianceType type;
    public float cookDuration;
    public float burnDuration;
}